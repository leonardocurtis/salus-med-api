using Isopoh.Cryptography.Argon2;
using SalusMedApi.Application.Interfaces.Security;

namespace SalusMedApi.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int TimeCost = 3;
    private const int MemoryCostKiB = 65536;
    private const int Parallelism = 1;
    private readonly Lazy<string> _dummyHash;
    public string DummyHash => _dummyHash.Value;

    public PasswordHasher() =>
        _dummyHash = new Lazy<string>(() => Hash("dummy-password-for-timing"));

    public string Hash(string password) =>
        Argon2.Hash(password, TimeCost, MemoryCostKiB, Parallelism, Argon2Type.HybridAddressing);

    public bool Verify(string password, string hash) => Argon2.Verify(hash, password);
}
