using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVMaze_Scrapper.Models;

namespace TVMaze_Scrapper.Services
{
    interface IRepositoryServices
    {
        void StoreMultipleShows(List<Show> Shows);

        void StoreShow(Show Show);

        void UpdateAllCast(List<Cast> CastMembers, int ShowId);

        void UpdateSingleCast(Cast Cast, int ShowId);

        void ClearShowData();

        List<Show> GetAllShows();

        List<Show> GetShowsByPage(int Page, int ShowsPerPage);
    }
}
