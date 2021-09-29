using dii_MovieCatalogSvc.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dii_MovieCatalogSvc.Features.SeedData
{
    public static class DataSeeding
    {
        /// <summary>
        /// Given the dot-deliminited name of a subdirectory of "DiiLegacy.Data" (e.g. "Assets.MovieMetadata")
        /// return a list of the JSON strings for each embedded resource in that subdirectory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<Guid,string>> GetJsonAssets(string directory) // e.g. "Assets.MovieMetadata"
        {
            string manifestModule = typeof(DataSeeding).Assembly.ManifestModule.ToString();      // "Dii_MovieCatalogSvc.dll"
            string root = Path.GetFileNameWithoutExtension(manifestModule);                      // "Dii_MovieCatalogSvc"
            string matchLeft = $"{root}.{directory}.";                              // e.g. "Dii_MovieCatalogSvc.Assets.MovieMetadata."

            var manifestResourceNames = typeof(DataSeeding).Assembly.GetManifestResourceNames(); // e.g. "Dii_MovieCatalogSvc.Assets.MovieMetadata.Rogue One A Star Wars Story (2016) 00000000000000000000000000000001.json", ...
            foreach (string name in manifestResourceNames.Where(n => n.StartsWith(matchLeft)))
            {
                string last37 = name.Substring(name.Length - 37); // e.g. "00000000000000000000000000000001.json"
                string idAsString = last37.Substring(0, 32); 
                Guid id = Guid.Parse(idAsString);
                using Stream stream = typeof(DataSeeding).Assembly.GetManifestResourceStream(name);
                using StreamReader sr = new StreamReader(stream);
                string content = sr.ReadToEnd();
                var tuple = new Tuple<Guid, string>(id, content);
                yield return tuple;
            }
        }

        public static IDictionary<Guid, Movie> GetSeedData()
        {
            var movies = new Dictionary<Guid, Movie>();
            foreach (var tuple in GetJsonAssets("Assets.MovieMetadata"))
            {
                Guid id = tuple.Item1;
                var movieMetadata = MovieMetadata.FromJson(tuple.Item2);
                var movie = new Movie
                {
                    MovieId = id,
                    Title = movieMetadata.Title,
                    MovieMetadata = movieMetadata
                };
                movies[id] = movie;
            }
            return movies;
        }
    }
}
