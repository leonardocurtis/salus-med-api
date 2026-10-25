using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Infrastructure.Extensions;

public static class ClinicQueryableExtensions
{
    extension(IQueryable<Clinic> query)
    {
        public IQueryable<Clinic> Active() => query.Where(d => d.Status == ClinicStatus.Active);

        public IQueryable<Clinic> ByPublicId(Guid publicId) =>
            query.Where(d => d.PublicId == publicId);

        public IQueryable<Clinic> ActiveByPublicId(Guid publicId) =>
            query.Active().ByPublicId(publicId);
    }
}
