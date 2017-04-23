using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultratimer
{
    public interface IRestService
    { 
        Task<List<Game>> GetGamesAsync();
        Task<Clues> GetFirstClueAndStoreNameAsync(int gameId, string participant);
        Task<Clues> GetClueFromNFCAsync(string participant, string HFCode);

    }
}
