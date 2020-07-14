using Nest;
using NuSearch.Domain.Data;
using NuSearch.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;

namespace NuSearch.Commands
{
	public class NestCommand : IElasticSearchCommand
	{
		private readonly ElasticClient elasticClient;
		private readonly NugetDumpReader nugetDumpReader;

		public NestCommand(ElasticClient elasticClient, NugetDumpReader nugetDumpReader)
		{
			this.elasticClient = elasticClient;
			this.nugetDumpReader = nugetDumpReader;
		}

		public void DeleteIndexIfExists()
		{
			if (elasticClient.Indices.Exists(new IndexExistsRequest("nusearch")).Exists)
				elasticClient.Indices.Delete("nusearch");
		}

		public void CreateIndex()
		{
			elasticClient.Indices.Create("nusearch", i => i
				.Settings(s => s
					.NumberOfShards(2)
					.NumberOfReplicas(0)
					.Setting("index.mapping.nested_objects.limit", 12000)
				)
				.Map<Package>(map => map
					.AutoMap()
					.Properties(ps => ps
						.Nested<PackageVersion>(n => n
							.Name(p => p.Versions.First())
							.AutoMap()
							.Properties(pps => pps
								.Nested<PackageDependency>(nn => nn
									.Name(pv => pv.Dependencies.First())
									.AutoMap()
								)
							)
						)
						.Nested<PackageAuthor>(n => n
							.Name(p => p.Authors.First())
							.AutoMap()
							.Properties(props => props
								.Text(t => t
									.Name(a => a.Name)
									.Fielddata()
								)
							)
						)
					)
				)
			);
		}

		public void InsertDocuments()
		{
			Console.WriteLine("Setting up a lazy xml files reader that yields packages...");
			var packages = nugetDumpReader.GetPackages();

			Console.Write("Indexing documents into Elasticsearch...");
			var waitHandle = new CountdownEvent(1);

			var bulkAll = elasticClient.BulkAll(packages, b => b
						.BackOffRetries(2)
						.BackOffTime("30s")
						.RefreshOnCompleted(true)
						.MaxDegreeOfParallelism(4)
						.Size(1000)
					);

			ExceptionDispatchInfo captureInfo = null;

			bulkAll.Subscribe(new BulkAllObserver(
				onNext: b => Console.Write("."),
				onError: e =>
				{
					captureInfo = ExceptionDispatchInfo.Capture(e);
					waitHandle.Signal();
				},
				onCompleted: () => waitHandle.Signal()
			));

			waitHandle.Wait();
			captureInfo?.Throw();
			Console.WriteLine($"Done.");

		}
	}
}
