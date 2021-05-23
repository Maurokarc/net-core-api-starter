using System;
using System.Linq;
using System.Reflection;

namespace NetCoreApi.Toolkit.Extensions
{
    public static class ClassExtension
    {
        public static TEntity PropertyAssignTo<TEntity>(this object tSrc) where TEntity : class, new()
        {
            TEntity tDst = new TEntity();

            if (tSrc.IsNull())
            {
                return tDst;
            }

            var srcProps = tSrc.GetType().GetProperties();
            var dstProps = typeof(TEntity).GetProperties();

            var intersectProps = dstProps.Where(dst => dst.CanWrite && srcProps.Any(src => src.Name.Equals(dst.Name, StringComparison.OrdinalIgnoreCase)));

            foreach (PropertyInfo prop in intersectProps)
            {
                var value = srcProps.FirstOrDefault(p => p.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)).GetValue(tSrc, null);
                prop.SetValue(tDst, value, null);
            }

            return tDst;
        }
        public static TDst PropertyAssignTo<TSrc, TDst>(this TSrc tSrc) where TSrc : class, new() where TDst : class, new()
        {
            if (tSrc.IsNull())
            {
                tSrc = new TSrc();
            }

            TDst tDst = new TDst();

            var srcProps = typeof(TSrc).GetProperties();
            var dstProps = typeof(TDst).GetProperties();
            var intersectProps = dstProps.Where(dst => dst.CanWrite && srcProps.Any(src => src.Name.Equals(dst.Name, StringComparison.OrdinalIgnoreCase)));

            foreach (PropertyInfo prop in intersectProps)
            {
                var value = srcProps.FirstOrDefault(p => p.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)).GetValue(tSrc, null);
                prop.SetValue(tDst, value, null);
            }

            return tDst;
        }

        public static void PropertyAssignTo<TSrc, TDst>(this TSrc tSrc, TDst tDst) where TSrc : class, new() where TDst : class, new()
        {
            if (tSrc.IsNull())
            {
                tSrc = new TSrc();
            }

            if (tDst.IsNull())
            {
                tDst = new TDst();
            }

            var srcProps = typeof(TSrc).GetProperties();
            var dstProps = typeof(TDst).GetProperties();
            var intersectProps = dstProps.Where(dst => dst.CanWrite && srcProps.Any(src => src.Name.Equals(dst.Name, StringComparison.OrdinalIgnoreCase)));

            foreach (PropertyInfo prop in intersectProps)
            {
                var value = srcProps.FirstOrDefault(p => p.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase)).GetValue(tSrc, null);
                prop.SetValue(tDst, value, null);
            }
        }

    }
}
