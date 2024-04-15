using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface IBoardRepository
    {
        object Board { get; }

        Task<List<Board>> GetAllAsync();
        Task<List<Board>> GetBoardsByWorkspaceIdAsync(int workspaceId);
        Task<Board?> GetByIdAsync(int id);
        Task<Board> CreateAsync(Board board);
        Task<Board?> UpdateAsync(int id,Board boardModel);
        Task<Board?> DeleteAsync(int id);
        Task<List<Board?>> DeleteBoardsAsync(int id);
        Task<List<Board>> DeleteBoardsByWorkspaceIdAsync(int workspaceId);
    }
}