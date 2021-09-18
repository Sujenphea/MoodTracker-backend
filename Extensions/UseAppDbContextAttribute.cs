using System.Reflection;
using MoodTrackerBackendCosmos.Data;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace MoodTrackerBackendCosmos.Extensions
{
    public class UseAppDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<AppDbContext>();
        }
    }
}
