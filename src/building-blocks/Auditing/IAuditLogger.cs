using System.Threading;
using System.Threading.Tasks;

namespace Auditing;

public interface IAuditLogger
{
    Task LogAsync(AuditEventRecord record, CancellationToken cancellationToken = default);
}
