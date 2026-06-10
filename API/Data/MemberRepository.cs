using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    public async Task<IReadOnlyList<Member?>> GetMebersAsync()
    {
        return await context.Members.ToListAsync();
    }

    public async Task<Member?> GetMebersByIdAsync(string id)
    {
        return await context.Members
         // .Include(x=>x.User)
          .FindAsync(id);
    }
    public async Task<Member?> GetMemberForUpdate(string id)
    {
        return await context.Members
           .Include(x=>x.User)
           .Include(x=>x.Photos)
          .SingleOrDefaultAsync(x=> x.Id ==id);
    }

    public async Task<IReadOnlyList<Photo>> GetPhotosFormmberAsync(string memberId)
    {
        return await context.Members
        .Where(s=>s.Id ==memberId)
        .SelectMany(s=>s.Photos)
        .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
       return  await context.SaveChangesAsync() >0;
    }

    public void Update(Member member)
    {
        context.Entry(member).State =EntityState.Modified;
    }
}
