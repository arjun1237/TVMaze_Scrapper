using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TVMaze_Scrapper.Models;

namespace TVMaze_Scrapper.Services
{
    public class RepositoryServices : IRepositoryServices
    {
        // stores numtiple show in DB
        public void StoreMultipleShows(List<Show> Shows)
        {
            MongoDBServices.InsertMultipleData(CollectionNames.Shows, Shows);
        }

        // stores single show in DB
        public void StoreShow(Show Show)
        {
            var collection = MongoDBServices.GetCollection<Show>(CollectionNames.Shows);
            var res = collection.ReplaceOne(x => x.id == Show.id, Show);
            if(res.MatchedCount == 0)
            {
                MongoDBServices.InsertSingleData(CollectionNames.Shows, Show);
            }
        }

        // Updates all the Cast Member info of the given show ID
        public void UpdateAllCast(List<Cast> CastMembers, int ShowId)
        {
            if(CastMembers == null)
            {
                return;
            }
            var collection = MongoDBServices.GetCollection<Show>("shows");
            var filter = Builders<Show>.Filter.Eq(x => x.id, ShowId);
            var update = Builders<Show>.Update.Set(x => x.cast, CastMembers);
            collection.UpdateOne(filter, update);
        }

        // Updates individual cast based on the show
        public void UpdateSingleCast(Cast Cast, int ShowId)
        {
            if(Cast == null)
            {
                return;
            }
            var collection = MongoDBServices.GetCollection<Show>("shows");
            var filter = Builders<Show>.Filter.Eq(x => x.id, ShowId);
            var Show = collection.Find(filter).FirstOrDefault();

            if(Show != null)
            {
                var CastMembers = Show.cast;
                if (CastMembers != null)
                {
                    var index = CastMembers.FindIndex(x => x.id == Cast.id);
                    CastMembers[index] = Cast;
                    var update = Builders<Show>.Update.Set(x => x.cast, CastMembers);
                    var res = collection.UpdateOne(filter, update);
                }
                else
                {
                    var update = Builders<Show>.Update.Set(x => x.cast, new List<Cast> { Cast });
                    var res = collection.UpdateOne(filter, update);
                }
            }

        }

        // removes all data from the shows collection
        public void ClearShowData()
        {
            MongoDBServices.ClearCollection("shows");
        }

        // all shows from the DB
        public List<Show> GetAllShows()
        {
            var collection = MongoDBServices.GetCollection<Show>(CollectionNames.Shows);
            var filter = Builders<Show>.Filter.Empty;

            return collection.Find(filter).ToList();
        }

        // Shows by page from DB
        public List<Show> GetShowsByPage(int Page, int ShowsPerPage)
        {
            var collection = MongoDBServices.GetCollection<Show>(CollectionNames.Shows);
            var filter = Builders<Show>.Filter.Empty;

            var shows = collection.Find(filter).Skip((Page - 1) * ShowsPerPage).Limit(ShowsPerPage).ToList();

            foreach(var Show in shows)
            {
                var Cast = Show.cast;

                var temp = (from c in Cast
                            let y = c.birthday
                            let x = y != null ? DateTime.ParseExact(y, "yyyy-MM-dd", CultureInfo.InvariantCulture) : (DateTime ?)null
                            orderby x       // birthday descending by age
                            select c).ToList();

                Show.cast = temp;
            }

            return shows;
        }
    }
}
