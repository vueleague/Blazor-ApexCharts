﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApexCharts
{
    public class ApexCandleSeries<TItem> : ApexBaseSeries<TItem>, IApexSeries<TItem> where TItem : class
    {
        [Parameter] public Func<TItem, decimal> Open { get; set; }
        [Parameter] public Func<TItem, decimal> High { get; set; }
        [Parameter] public Func<TItem, decimal> Low { get; set; }
        [Parameter] public Func<TItem, decimal> Close { get; set; }
        [Parameter] public Func<ListPoint<TItem>, object> OrderBy { get; set; }
        [Parameter] public Func<ListPoint<TItem>, object> OrderByDescending { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Chart.AddSeries(this);
        }

        public ChartType GetChartType()
        {
            return ChartType.Candlestick;
        }

        public IEnumerable<IDataPoint<TItem>> GetData()
        {
            var data = Items
         .Select(d => new ListPoint<TItem>
         {
             X = XValue.Invoke(d),
             Y = new List<decimal?>
             {
                       Open.Invoke(d),
                       High.Invoke(d),
                       Low.Invoke(d),
                       Close.Invoke(d)
             },
             Items = Items
         });

            if (OrderBy != null)
            {
                data = data.OrderBy(OrderBy);
            }
            else if (OrderByDescending != null)
            {
                data = data.OrderByDescending(OrderByDescending);
            }

            return data;

        }

        public void Dispose()
        {
            Chart.RemoveSeries(this);
        }

    }
}