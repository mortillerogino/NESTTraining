using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Nest;
using NuSearch.Domain.Model;

namespace NuSearch.Domain
{
	public static class NuSearchConfiguration
	{
		public static ElasticClient GetClient() => new ElasticClient(_connectionSettings);

		static NuSearchConfiguration()
		{
			_connectionSettings = new ConnectionSettings(CreateUri(9200))
				.DefaultMappingFor<Package>(i => i.IndexName("nusearch"))
				.PrettyJson();
		}

		private static readonly ConnectionSettings _connectionSettings;

		public static string LiveIndexAlias => "nusearch";

		public static string OldIndexAlias => "nusearch-old";

		public static Uri CreateUri(int port)
		{
			return new Uri($"http://localhost:{port}");
		}
	
		public static string CreateIndexName() => $"{LiveIndexAlias}-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss}";

		public static string PackagePath => 
			RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"C:\Work\data\nuget-data" : "/nuget-data";
	}
}
