using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TVMaze_Scrapper.Models;
using TVMaze_Scrapper.Services;

namespace TVMaze_Scrapper
{
    [Produces("application/json")]
    [Route("api/ScrapeStore")]
    public class ScrapeStoreController : Controller
    {
        private ScraperServices ScraperServices;
        private RepositoryServices RepositoryServices;

        public ScrapeStoreController()
        {
            ScraperServices = new ScraperServices();
            RepositoryServices = new RepositoryServices();
        }

        [HttpGet]
        [Route("GetShows")]
        // gets shows from first page with cast members sorted by birthday descending (by age).
        public List<Show> GetShows()  
        {
            return GetShows(1, 10);
        }

        [HttpGet]
        [Route("GetShows/{Page}/{ShowsPerPage}")]
        // extacts show by page with cast members sorted by birthday descending (by age).
        public List<Show> GetShows(int Page, int ShowsPerPage)
        {
            if(Page > 0)  // Page number must start from 1
            {
                return RepositoryServices.GetShowsByPage(Page, ShowsPerPage);
            }
            return new List<Show>();
        }

        [HttpGet]
        [Route("ScrapeAndStoreAllShows")]
        // scrape all shows and store in DB
        public List<Show> ScrapeAndStoreAllShows()
        {
            List<Show> Shows = ScraperServices.ScrapeAllShows(); // scraper
            if (Shows != null)
            {
                RepositoryServices.ClearShowData();  // clear collection
                RepositoryServices.StoreMultipleShows(Shows);  // storage
            }
            return Shows;
        }

        [HttpGet]
        [Route("ScrapeAndStoreShowById/{id}")]
        // scrape an individual show and store in DB
        public Show ScrapeAndStoreShowById(int id)
        {
            Show Show = ScraperServices.ScrapeShowById(id); // scraper
            if(Show != null)
            {
                RepositoryServices.StoreShow(Show); // storage
            }            
            return Show;
        }

        [HttpGet]
        [Route("ScrapeAndStoreCastByShowId/{id}")]
        // scrape all cast of a show and store in DB
        public List<Cast> ScrapeAndStoreCastByShowId(int id)
        {
            List<Cast> CastMembers =  ScraperServices.ScrapeCastByShowId(id);  //scraper

            if(CastMembers != null)
            {
                RepositoryServices.UpdateAllCast(CastMembers, id);  // storage
            }
            return CastMembers;
        }

        [HttpGet]
        [Route("ScrapeAndStoreCastByShowAndCastId/{ShowId}/{CastId}")]
        // scrape a cast memeber by the show and store in DB
        public Cast ScrapeAndStoreCastByShowAndCastId(int ShowId, int CastId)
        {
            Cast Cast = ScraperServices.ScrapeSingleCastByShowAndCastId(ShowId, CastId);  //scraper

            if (Cast != null)
            {
                RepositoryServices.UpdateSingleCast(Cast, ShowId);  // storage
            }
            return Cast;
        }
    }
}