using System.Collections.Generic;

namespace LogTail.Data
{
    public interface IDAO<TEntity, TPk>
    {
        TPk GetMaxPk();

        IList<TEntity> ListAfter(TPk pk);

        /// <summary>
        /// List the records of specific date
        /// </summary>
        /// <param name="dateStr">yyyyMMdd</param>
        /// <returns>list of the date</returns>
        IList<TEntity> ListByDate(string dateStr);

        /// <summary>
        /// Delete records before the date 
        /// </summary>
        /// <param name="dateStr"></param>
        void DeleteBefore(string dateStr);
    }
}
