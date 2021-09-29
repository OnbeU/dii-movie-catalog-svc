using dii_MovieCatalogSvc.Features.SeedData;
using System;
using System.Collections.ObjectModel;

namespace dii_MovieCatalogSvc.Data
{
    public class MovieCatalogSvcContext
    {
        public MovieCatalogSvcContext()
        {
            var movies = DataSeeding.GetSeedData();
            Movies = new ReadOnlyDictionary<Guid, Movie>(movies);
        }

        public readonly ReadOnlyDictionary<Guid, Movie> Movies;
    }
}
