using System;
using Vostok.Clusterclient.Core.Model;
using Vostok.Commons.Time;

namespace Vostok.Clusterclient.Singular.Helpers
{
    // TODO(lunev.d): why this class is internal in clusterlient?
    internal class RequestTimeBudget : TimeBudget, IRequestTimeBudget
    {
        private static readonly TimeSpan BudgetPrecision = TimeSpan.FromMilliseconds(15);

        public new static RequestTimeBudget Infinite = new RequestTimeBudget(TimeSpan.MaxValue, TimeSpan.Zero);

        private RequestTimeBudget(TimeSpan budget, TimeSpan precision)
            : base(budget, precision)
        {
        }

        public new static RequestTimeBudget StartNew(TimeSpan budget, TimeSpan? precision = null)
        {
            var timeBudget = new RequestTimeBudget(budget, precision ?? BudgetPrecision);
            timeBudget.Start();
            return timeBudget;
        }
    }
}