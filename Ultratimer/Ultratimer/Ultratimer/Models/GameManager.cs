using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultratimer
{
    public class GameManager
    {
        IRestService restService;

        public GameManager(IRestService service)
        {
            restService = service;
        }

        public Task<List<Game>> GetGamesAsync()
        {
            try
            {
                return restService.GetGamesAsync();
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public Task<Clues> GetFirstClueAndStoreNameAsync(int gameId, string participant)
        {
            try
            {
                return restService.GetFirstClueAndStoreNameAsync(gameId, participant);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public Task<Clues> GetClueFromNFCAsync(string participant, string HFCode)
        {
            try
            {
                return restService.GetClueFromNFCAsync( participant, HFCode);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
       
    }        
}
