using ZR.Mall.Model;
using ZR.Mall.Model.Dto;
using ZR.Mall.Service.IService;

namespace ZR.Mall.Service
{
    /// <summary>
    /// 商品规格Service业务层处理
    /// </summary>
    [AppService(ServiceType = typeof(IProductSpecService))]
    public class ProductSpecService : BaseService<ProductSpec>, IProductSpecService
    {
        /// <summary>
        /// 查询商品规格列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public PagedInfo<ProductSpecDto> GetList(ShoppingProductSpecQueryDto parm)
        {
            var predicate = QueryExp(parm);

            var response = Queryable()
                .Where(predicate.ToExpression())
                .ToPage<ProductSpec, ProductSpecDto>(parm);

            return response;
        }

        /// <summary>
        /// 添加商品规格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ProductSpec AddShoppingProductspec(ProductSpec model)
        {
            return Insertable(model).ExecuteReturnEntity();
        }

        /// <summary>
        /// 修改商品规格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long UpdateShoppingProductspec(ProductSpec model)
        {
            return Update(model, true);
        }

        /// <summary>
        /// 查询导出表达式
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        private static Expressionable<ProductSpec> QueryExp(ShoppingProductSpecQueryDto parm)
        {
            var predicate = Expressionable.Create<ProductSpec>();

            return predicate;
        }

        public long DeleteSpecByProductId(long productId)
        {
            return Deleteable()
                .Where(f => f.ProductId == productId)
                .EnableDiffLogEvent("删除商品规格")
                .ExecuteCommand();
        }
    }
}