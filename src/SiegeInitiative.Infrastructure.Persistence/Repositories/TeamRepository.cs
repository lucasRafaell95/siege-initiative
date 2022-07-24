using SiegeInitiative.Domain.Entities;
using SiegeInitiative.Domain.Persistence.Repositories;
using SiegeInitiative.Infrastructure.Persistence.Core;

namespace SiegeInitiative.Infrastructure.Persistence.Repositories;

public sealed class TeamRepository : Repository<Team, int>, ITeamRepository
{
    public TeamRepository(SiegeDbContext context) : base(context) { }
}