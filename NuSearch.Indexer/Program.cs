using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Xml.Xsl;
using Nest;
using NuSearch.Domain;
using NuSearch.Domain.Data;
using NuSearch.Domain.Model;

namespace NuSearch.Indexer
{
	class Program
	{
		private static ElasticClient Client { get; set; }
		private static NugetDumpReader DumpReader { get; set; }

		static void Main(string[] args)
		{
			Client = NuSearchConfiguration.GetClient();
			string directory = args.Length > 0 && !string.IsNullOrEmpty(args[0])
				? args[0]
				: NuSearchConfiguration.PackagePath;
			DumpReader = new NugetDumpReader(directory);

			DeleteIndexIfExists();
			CreateIndex();
			IndexDumps();

			Console.WriteLine("Press Enter to continue");
			Console.Read();
		}

		static void IndexDumps()
		{
			Console.WriteLine("Setting up a lazy xml files reader that yields packages...");
			var packages = DumpReader.GetPackages();

			Console.Write("Indexing documents into Elasticsearch...");
			var waitHandle = new CountdownEvent(1);

			var sw = Stopwatch.StartNew();

			var bulkAll = Client.BulkAll(packages, b => b
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
			sw.Stop();
			Console.WriteLine($"Done. Elapsed {sw.ElapsedMilliseconds}");

		}

		static void DeleteIndexIfExists()
		{
			if (Client.Indices.Exists(new IndexExistsRequest("nusearch")).Exists)
				Client.Indices.Delete("nusearch");
		}

		static void CreateIndex()
		{
			Client.Indices.Create("nusearch", i => i
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

		//private static void SwapAlias()
		//{
		//	var indexExists = Client.Indices.Exists(NuSearchConfiguration.LiveIndexAlias).Exists;

		//	Client.Indices.BulkAlias(aliases =>
		//	{
		//		if (indexExists)
		//			aliases.Add(a => a
		//				.Alias(NuSearchConfiguration.OldIndexAlias)
		//				.Index(Client.GetIndicesPointingToAlias(NuSearchConfiguration.LiveIndexAlias).First())
		//			);

		//		return aliases
		//			.Remove(a => a.Alias(NuSearchConfiguration.LiveIndexAlias).Index("*"))
		//			.Add(a => a.Alias(NuSearchConfiguration.LiveIndexAlias).Index(CurrentIndexName));
		//	});

		//	var oldIndices = Client.GetIndicesPointingToAlias(NuSearchConfiguration.OldIndexAlias)
		//		.OrderByDescending(name => name)
		//		.Skip(2);

		//	foreach (var oldIndex in oldIndices)
		//		Client.Indices.Delete(oldIndex);
		//}
	}
}
 