using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVMaze_Scrapper.Models;

namespace TVMaze_Scrapper.Services
{
    public class ScraperServices : IScraperServices
    {
        // scrape all show from TV maze api
        public List<Show> ScrapeAllShows()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew(); // for keeping track on rate limit 
            List<Show> Shows = new List<Show>();
            var URL = $"http://api.tvmaze.com/shows";
            List<ScrapedShow> ScrapedShows = HTTPServices.GetHTTPService<List<ScrapedShow>>(URL);

            if (ScrapedShows != null)
            {
                ScrapedShows.ForEach(x => Shows.Add(new Show { id = x.id, name = x.name })); // transfer data
            }

            var Count = 1; // keeping track on no of API calls

            foreach (var Show in Shows)
            {
                var ShowID = Show.id;
                List<Cast> ScrapedCast = null;
                if (Count < 20) // because rate limit : 20 calls in 10 sec
                {
                    ScrapedCast = ScrapeCastByShowId(ShowID);
                    Count++;
                }
                else
                {
                    if (watch.ElapsedMilliseconds < 10000)
                    {
                        System.Threading.Thread.Sleep(10000 - ((int)watch.ElapsedMilliseconds)); // delay to respect rate limit
                    }
                    watch.Restart();
                    ScrapedCast = ScrapeCastByShowId(ShowID);
                    Count = 1;
                }

                // fill in cast member data into model
                if (ScrapedCast != null)
                {
                    Show.cast = ScrapedCast;
                }
            }

            return Shows;
        }

        // scrape show by ID from TV maze api
        public Show ScrapeShowById(int id)
        {
            Show Show = new Show();
            var URL = $"http://api.tvmaze.com/shows/{id}";
            ScrapedShow ScrapedShow = HTTPServices.GetHTTPService<ScrapedShow>(URL);

            if (ScrapedShow != null)
            {
                Show = new Show { id = ScrapedShow.id, name = ScrapedShow.name }; // transfer data
            }

            List<Cast> CastMembers = ScrapeCastByShowId(id);
            if(CastMembers != null)
            {
                Show.cast = CastMembers;
            }

            return Show;
        }

        // scrape all cast by given show ID from TV maze api
        public List<Cast> ScrapeCastByShowId(int id)
        {
            var URL = $"http://api.tvmaze.com/shows/{id}/cast";
            List<ScrapedCast> Cast = HTTPServices.GetHTTPService<List<ScrapedCast>>(URL);
            List<Cast> CastMembers = new List<Cast>();
            if(Cast != null)
            {
                foreach(var SingleCast in Cast)
                {
                    var Person = SingleCast.person;
                    if (SingleCast.person != null)
                    {
                        CastMembers.Add(Person);
                    }
                }
            }
            return CastMembers;
        }

        // scrape cast by cast ID and show ID from TV maze api
        public Cast ScrapeSingleCastByShowAndCastId(int ShowId, int CastId)
        {
            var URL = $"http://api.tvmaze.com/shows/{ShowId}/cast";
            List<ScrapedCast> Cast = HTTPServices.GetHTTPService<List<ScrapedCast>>(URL);
            ScrapedCast Individual = Cast.SingleOrDefault(x => x.person.id == CastId);
            return Individual == null ? null : Individual.person;
        }
    }
}
