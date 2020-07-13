using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Elasticsearch.Net;
using Nest;
using NuSearch.Domain.Model;

namespace NuSearch.Domain
{
	public static class NuSearchConfiguration
	{
		public static ElasticClient GetClient() => new ElasticClient(_connectionSettings);

		static NuSearchConfiguration()
		{
			var config = AppSettings.Configuration;
			var tea = config["Port"];

			if (!int.TryParse(config["Port"], out int port))
			{
				throw new Exception("Invalid Port on AppSettings");
			}

			_connectionSettings = new ConnectionSettings(CreateUri(port))
				.BasicAuthentication(config["Username"], config["Password"])
				.ServerCertificateValidationCallback(CertificateValidations.AllowAll)
				.DefaultMappingFor<Package>(i => i.IndexName("nusearch"))
				.PrettyJson();
		}

		private static readonly ConnectionSettings _connectionSettings;

		public static string LiveIndexAlias => "nusearch";

		public static string OldIndexAlias => "nusearch-old";

		public static Uri CreateUri(int port)
		{
			return new Uri($"https://localhost:{port}");
		}
	
		public static string CreateIndexName() => $"{LiveIndexAlias}-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss}";

		public static string PackagePath => 
			RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"C:\Work\data\nuget-data" : "/nuget-data";

	}
}
