

using backend.Data;
using backend.DTOs.Background.Input;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class BackgroundRepository : IBackgroundRepository
    {
        private readonly ApplicationDBContext _context;

        public BackgroundRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        //GET ALL
        public async Task<List<Background>> GetAllBackgroundsAsync()
        {
            return await _context.Background.ToListAsync();
        }
        
        //GET BY ID
        public async Task<Background?> GetBackgroundByIdAsync(int id)
        {
            return await _context.Background.FirstOrDefaultAsync(i => i.BackgroundId == id);
        }

        //GET BY CreatorID
        public async Task<List<Background>> GetBackgroundsByCreatorIdAsync(string creatorId)
        {
            return await _context.Background.Where(b => b.CreatorId.Equals(creatorId)).ToListAsync();
        }
        
        //GET ACTIVE BACKGROUNDS
        public async Task<List<Background>> GetActiveBackgroundsAsync()
        {
            return await _context.Background.Where(b => b.IsActive == true).ToListAsync();
        }
        
        //CREATE
        public async Task<Background> CreateBackgroundAsync(Background backgroundModel)
        {
            await _context.Background.AddAsync(backgroundModel);
            await _context.SaveChangesAsync();
            return backgroundModel;
        }

        //UPDATE
        public async Task<Background?> UpdateBackgroundByIdAsync(UpdateBackgroundDto backgroundDto)
        {
            var existingBackground = await _context.Background.FirstOrDefaultAsync(i => i.BackgroundId == backgroundDto.BackgroundId);

            if (existingBackground == null)
                return null;

            existingBackground.Title = backgroundDto.Title;
            existingBackground.IsActive = backgroundDto.IsActive;
            existingBackground.ImageUrl = backgroundDto.ImageUrl;

            await _context.SaveChangesAsync();
            return existingBackground;
        }

        //DELETE BY ID
        public async Task<Background?> DeleteBackgroundByIdAsync(int id)
        {
            var backgroundModel = await _context.Background.FirstOrDefaultAsync(i => i.BackgroundId == id);

            if (backgroundModel == null)
                return null;

            _context.Background.Remove(backgroundModel);
            await _context.SaveChangesAsync();
            return backgroundModel;
        }
        
        //DELETE BY CreatorID
        public async Task<List<Background>> DeleteBackgroundsByCreatorId(string creatorId)
        {
            var backgrounds = await _context.Background.Where(b => b.CreatorId.Equals(creatorId)).ToListAsync();

            if (backgrounds.Count == 0)
                return null;
            
            _context.Background.RemoveRange(backgrounds);
            await _context.SaveChangesAsync();
            return backgrounds;
        }

        public async Task<bool> BackgroundExists(int backgroundId)
        {
            return await _context.Background.AnyAsync(i => i.BackgroundId == backgroundId);
        }
    }
}

