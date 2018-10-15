using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVMaze_Scrapper.Models;

namespace TVMaze_Scrapper.Services
{
    public interface IScraperServices
    {
        List<Show> ScrapeAllShows();

        Show ScrapeShowById(int Id);

        List<Cast> ScrapeCastByShowId(int id);

        Cast ScrapeSingleCastByShowAndCastId(int ShowID, int CastID);
    }
}
