using System;
using API.Entities;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<bool> SaveAllAsync();
    Task<IReadOnlyList<Member>>GetMebersAsync();
    Task<Member?>GetMebersByIdAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotosFormmberAsync(string memberId);


}
