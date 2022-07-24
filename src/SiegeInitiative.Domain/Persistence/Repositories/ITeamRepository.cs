using SiegeInitiative.Domain.Entities;
using SiegeInitiative.Domain.Persistence.Repositories.Base;

namespace SiegeInitiative.Domain.Persistence.Repositories;

/// <summary>
/// Team repository interface
/// </summary>
public interface ITeamRepository : IRepository<Team, int> { }