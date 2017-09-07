using System;
using System.Collections.Generic;
using System.IO;

namespace FileQuery.Core.Filter
{
    public class FileDateModifiedFilter : FileQueryFilter
    {
        private List<DateTime> modifiedTimes = new List<DateTime>();

        public FileDateModifiedFilter(DateTime date, FilterOperator op)
            : base(op)
        {
            modifiedTimes.Add(date);
        }

        public FileDateModifiedFilter(string sDate, FilterOperator op)
            : base(op)
        {
            try
            {
                modifiedTimes.Add(Convert.ToDateTime(sDate).Date);
            }
            catch (Exception ex)
            {
                throw new FileQueryException("Error parsing date: " + sDate + ": " + ex.Message);
            }
        }

        public FileDateModifiedFilter(params string[] dates)
            : this(dates[0], FilterOperator.In)
        {
            for (var i = 1; i < dates.Length; i++)
            {
                modifiedTimes.Add(Convert.ToDateTime(dates[i]).Date);
            }
        }

        public override bool AcceptFile(FileInfo file)
        {
            foreach (var modTime in modifiedTimes)
            {
                if (TestDate(file.LastWriteTime.Date, modTime))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TestDate(DateTime fileTime, DateTime modTime)
        {
            switch (FilterOperator)
            {
                case FilterOperator.Equal:
                case FilterOperator.In:
                    return fileTime == modTime;
                case FilterOperator.GreaterThan:
                    return fileTime > modTime;
                case FilterOperator.GreaterThanEqual:
                    return fileTime >= modTime;
                case FilterOperator.LessThan:
                    return fileTime < modTime;
                case FilterOperator.LessThanEqual:
                    return fileTime <= modTime;
                case FilterOperator.NotEqual:
                    return fileTime != modTime;
                default:
                    throw new FileQueryException("Invalid size filter operator: " + FilterOperator);
            }
        }

        public override string Name
        {
            get { return "modified"; }
        }

        public override string ToString()
        {
            return base.ToString() + ": " + string.Join(",", modifiedTimes);
        }
    }
}
