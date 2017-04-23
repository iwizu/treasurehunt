using nfcmai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls; //For SortBy method

namespace nfcmai
{
    public class GamesResultSet
    {
        public List<UGames> GetResult(string search, string sortOrder, int start, int length, List<UGames> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<UGames> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<UGames> FilterResult(string search, List<UGames> dtResult, List<string> columnFilters)
        {
            IQueryable<UGames> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower()))) );

            return results;
        }
    }
    public class TipsResultSet
    {
        public List<UTips> GetResult(string search, string sortOrder, int start, int length, List<UTips> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<UTips> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<UTips> FilterResult(string search, List<UTips> dtResult, List<string> columnFilters)
        {
            IQueryable<UTips> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Titlos != null && p.Titlos.ToLower().Contains(search.ToLower()))));

            return results;
        }
    }
    public class ParticipantsResultSet
    {
        public List<UParticipants> GetResult(string search, string sortOrder, int start, int length, List<UParticipants> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<UParticipants> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<UParticipants> FilterResult(string search, List<UParticipants> dtResult, List<string> columnFilters)
        {
            IQueryable<UParticipants> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower()))));

            return results;
        }
    }
}