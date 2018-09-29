using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.COM.Entity
{
    /// <summary>
    /// 产品信息主体信息实体
    /// </summary>
    public class ProductInfoResponseDTO
    {
        /// <summary>
        /// 产品主体信息（包含套餐基本信息列表）
        /// </summary>
        public ProductDetailInfoDTO ProductInfo { get; set; }
        /// <summary>
        /// 产品行程信息
        /// </summary>
        public ProductSegmentDTO Segment { get; set; }

        /// <summary>
        /// 可选项（自费项目）
        /// </summary>
        public List<ProductResourceInfoDTO> Options { get; set; }
    }

    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductDetailInfoDTO
    {
        /// <summary>
        /// 产品基本信息
        /// </summary>
        public ProductBasicInfoDTO BasicInfo { get; set; }

        /// <summary>
        /// 动态属性
        /// </summary>
        public List<ProductPropertyDTO> ProductProps { get; set; }

        /// <summary>
        /// 费用说明
        /// </summary>
        public FeeDescriptionDTO FeeDescription { get; set; }

        /// <summary>
        /// 预订须知
        /// </summary>
        public BookAttentionDTO BookAttention { get; set; }

        /// <summary>
        /// 推荐活动
        /// </summary>
        public List<ProductRecommendActivityDTO> RecommendActivitys { get; set; }

        /// <summary>
        /// 套餐信息
        /// </summary>
        public List<ItemBasicInfoDTO> ItemList { get; set; }
    }

    /// <summary>
    /// 产品基本信息
    /// </summary>
    public class ProductBasicInfoDTO
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商户产品类型Id
        /// </summary>
        public int GroupProductTypeId { get; set; }

        /// <summary>
        /// 产品类型Id(1：跟团游；2：国内短途；3：国内长途；4：海外短途；5：海外长途；6：自由行；7：国内自由行；8：海外自由行；10：酒店；11：景点；12：接机；13：主题乐园；14：一日游；22：送机；) 
        /// 原来12接送机改成接机，21接机去掉
        /// </summary>
        public int ProductTypeId { get; set; }

        /// <summary>
        /// 产品类型名称
        /// </summary>
        public string ProductTypeName { get; set; }

        /// <summary>
        /// 出发城市
        /// </summary>
        public CityDTO StartCity { get; set; }

        /// <summary>
        /// 目的地城市Id
        /// </summary>
        public int DestCityId { get; set; }

        /// <summary>
        /// 目的城市名称
        /// </summary>
        public string DestCityName { get; set; }

        /// <summary>
        /// 目的地国家Id
        /// </summary>
        public int DestCountryId { get; set; }

        /// <summary>
        /// 目的国家名称
        /// </summary>
        public string DestCountryName { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? LngCode { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? LatCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 产品推荐
        /// </summary>
        public string Recommendation { get; set; }

        /// <summary>
        /// 特色推荐
        /// </summary>
        public string Recommendation1 { get; set; }

        /// <summary>
        /// 特色推荐
        /// </summary>
        public string Recommendation2 { get; set; }

        /// <summary>
        /// 特色推荐
        /// </summary>
        public string Recommendation3 { get; set; }

        /// <summary>
        /// 产品经理
        /// </summary>
        public string PMId { get; set; }

        /// <summary>
        /// 途经城市
        /// </summary>
        public List<CityDTO> PassCitys { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public List<ProductImageDTO> Images { get; set; }

        /// <summary>
        /// 线路信息
        /// </summary>
        public ItineraryDTO ProductItineraryInfo { get; set; }

        /// <summary>
        /// 产品形态,1：单团，2：套餐
        /// </summary>
        public int ProductForm { get; set; }

        /// <summary>
        /// 起价
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// 起价
        /// </summary>
        public decimal? MinCPrice { get; set; }
    }

    /// <summary>
    /// 产品图片
    /// </summary>
    public class ProductImageDTO
    {
        /// <summary>
        /// 图片Url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public int FrontCover { get; set; }
    }

    /// <summary>
    /// 行程资源（景点）
    /// </summary>
    public class ProductResourceDTO
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        public int ResourceTypeId { get; set; }

        /// <summary>
        /// 资源Id（产品ID）
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// 资源名称（产品名称）
        /// </summary>
        public string ResourceName { get; set; }
    }


    /// <summary>
    /// 预订须知
    /// </summary>
    public class BookAttentionDTO
    {
        /// <summary>
        /// 预订说明
        /// </summary>
        public string Notice { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string OperateDesc { get; set; }

        /// <summary>
        /// 退改规则(0：不支持退款；1：支持协商退款（限使用日期前）；2：支持随时退款（限使用日期前）；3：其他（BackRulesDesc为描述文字）)
        /// </summary>
        public int BackRules { get; set; }

        /// <summary>
        /// 退改规则描述
        /// </summary>
        public string BackRulesDesc { get; set; }

        /// <summary>
        /// 人群限制
        /// </summary>
        public CrowdCtrlDTO CrowdCtrlInfo { get; set; }
    }

    /// <summary>
    /// 人群限制
    /// </summary>
    public class CrowdCtrlDTO
    {
        /// <summary>
        /// 需现询确认的最少人数
        /// </summary>
        public int? MinUsers_Confirm { get; set; }

        /// <summary>
        /// 需现询确认的最大人数
        /// </summary>
        public int? MaxUsers_Confirm { get; set; }

        /// <summary>
        /// 不接收的最小年龄
        /// </summary>
        public int? MinAge_No { get; set; }

        /// <summary>
        /// 不接收的最大年龄
        /// </summary>
        public int? MaxAge_No { get; set; }

        /// <summary>
        /// 需要签署健康协议的最小年龄
        /// </summary>
        public int? MinAge_Agreement { get; set; }

        /// <summary>
        /// 正常预订的最小年龄
        /// </summary>
        public int? MinAge_Yes { get; set; }

        /// <summary>
        /// 正常预订的最大年龄
        /// </summary>
        public int? MaxAge_Yes { get; set; }

        /// <summary>
        /// 是否不接收外籍游客
        /// </summary>
        public int? Foreigner_No { get; set; }

        /// <summary>
        /// 地域限制-包含的地域
        /// </summary>
        public List<CityDTO> RegionLimit_Yes { get; set; }

        /// <summary>
        /// 地域限制-不包含的地域
        /// </summary>
        public List<CityDTO> RegionLimit_No { get; set; }
    }

    /// <summary>
    /// 费用包含VO
    /// </summary>
    public class FeeDescriptionDTO
    {
        #region 结构化

        #region 交通
        private string _traffic_MultSelect1 = "";
        /// <summary>
        /// 交通 - 选项1
        /// </summary>
        public string Traffic_MultSelect1
        {
            get { return _traffic_MultSelect1; }
            set { _traffic_MultSelect1 = value; }
        }

        private string _traffic_Way = "往返";

        /// <summary>
        /// 交通方式
        /// </summary>
        public string Traffic_Way
        {
            get { return _traffic_Way; }
            set { _traffic_Way = value; }
        }

        private string _traffic_IsTax = "含税费";
        /// <summary>
        /// 交通税费
        /// </summary>
        public string Traffic_IsTax
        {
            get { return _traffic_IsTax; }
            set { _traffic_IsTax = value; }
        }

        private string _traffic_MultSelect2 = "";

        /// <summary>
        /// 交通 - 选项2
        /// </summary>
        public string Traffic_MultSelect2
        {
            get { return _traffic_MultSelect2; }
            set { _traffic_MultSelect2 = value; }
        }

        private string _traffic_TransferAddr = "";
        /// <summary>
        /// 交通-接送机地址
        /// </summary>
        public string Traffic_TransferAddr
        {
            get { return _traffic_TransferAddr; }
            set { _traffic_TransferAddr = value; }
        }

        private string _traffic_Transfer = "接送机";
        /// <summary>
        /// 交通-接送机
        /// </summary>
        public string Traffic_Transfer
        {
            get { return _traffic_Transfer; }
            set { _traffic_Transfer = value; }
        }

        private string _traffic_MultSelect3 = "";

        /// <summary>
        /// 交通 - 选项3
        /// </summary>
        public string Traffic_MultSelect3
        {
            get { return _traffic_MultSelect3; }
            set { _traffic_MultSelect3 = value; }
        }

        private string _traffic_TourSpotCar = "";
        /// <summary>
        /// 交通-景区用车
        /// </summary>
        public string Traffic_TourSpotCar
        {
            get { return _traffic_TourSpotCar; }
            set { _traffic_TourSpotCar = value; }
        }

        private string _traffic_MultSelect4 = "";
        /// <summary>
        /// 交通 - 选项4
        /// </summary>
        public string Traffic_MultSelect4
        {
            get { return _traffic_MultSelect4; }
            set { _traffic_MultSelect4 = value; }
        }

        private string _traffic_Other = "";
        /// <summary>
        /// 交通其他
        /// </summary>
        public string Traffic_Other
        {
            get { return _traffic_Other; }
            set { _traffic_Other = value; }
        }
        #endregion

        #region 住宿
        private string _hotel_MultSelect1 = "";
        /// <summary>
        /// 住宿-选项1
        /// </summary>
        public string Hotel_MultSelect1
        {
            get { return _hotel_MultSelect1; }
            set { _hotel_MultSelect1 = value; }
        }

        private string _hotel_MultSelect2 = "";
        /// <summary>
        /// 住宿-选项2
        /// </summary>
        public string Hotel_MultSelect2
        {
            get { return _hotel_MultSelect2; }
            set { _hotel_MultSelect2 = value; }
        }

        private string _hotel_HotelName = "";
        /// <summary>
        /// 住宿-酒店名称
        /// </summary>
        public string Hotel_HotelName
        {
            get { return _hotel_HotelName; }
            set { _hotel_HotelName = value; }
        }

        private string _hotel_NightCount = "";
        /// <summary>
        /// 住宿-几晚
        /// </summary>
        public string Hotel_NightCount
        {
            get { return _hotel_NightCount; }
            set { _hotel_NightCount = value; }
        }
        #endregion

        #region 用餐
        private string _dines_MultSelect = "";
        /// <summary>
        /// 用餐-选项
        /// </summary>
        public string Dines_MultSelect
        {
            get { return _dines_MultSelect; }
            set { _dines_MultSelect = value; }
        }

        private string _dines_Desc = "";
        /// <summary>
        /// 用餐-描述
        /// </summary>
        public string Dines_Desc
        {
            get { return _dines_Desc; }
            set { _dines_Desc = value; }
        }

        #endregion

        #region 门票
        private string _tickit_MultSelect = "";
        /// <summary>
        /// 门票-选项
        /// </summary>
        public string Tickit_MultSelect
        {
            get { return _tickit_MultSelect; }
            set { _tickit_MultSelect = value; }
        }

        private string _tickit_Desc = "";
        /// <summary>
        /// 门票-描述
        /// </summary>
        public string Tickit_Desc
        {
            get { return _tickit_Desc; }
            set { _tickit_Desc = value; }
        }
        #endregion

        #region 导服
        private string _guideServe_SingleSelect = "";
        /// <summary>
        /// 导服-选项（0：无；1：当地中文导游；2：当地英文导游；3：全陪和当地中文导游）
        /// </summary>
        public string GuideServe_SingleSelect
        {
            get { return _guideServe_SingleSelect; }
            set { _guideServe_SingleSelect = value; }
        }

        private string _guideServe_LocalChinese = "";
        /// <summary>
        /// 导服-当地中文导游
        /// </summary>
        public string GuideServe_LocalChinese
        {
            get { return _guideServe_LocalChinese; }
            set { _guideServe_LocalChinese = value; }
        }

        private string _guideServe_LocalEnglish = "";
        /// <summary>
        /// 导服-当地英文导游
        /// </summary>
        public string GuideServe_LocalEnglish
        {
            get { return _guideServe_LocalEnglish; }
            set { _guideServe_LocalEnglish = value; }
        }

        private string _guideServe_All = "";
        /// <summary>
        /// 导服---全陪和当地中文导游
        /// </summary>
        public string GuideServe_All
        {
            get { return _guideServe_All; }
            set { _guideServe_All = value; }
        }
        #endregion

        #region 儿童标准
        private string _selectChildNorm = "";
        /// <summary>
        /// 儿童标准-选择（0：无；1：年龄；2：身高；3：其他）
        /// </summary>
        public string ChildNorm_SingleSelect
        {
            get { return _selectChildNorm; }
            set { _selectChildNorm = value; }
        }

        private string _childNorm_MinAge = "";
        /// <summary>
        /// 儿童标准-年龄最小
        /// </summary>
        public string ChildNorm_MinAge
        {
            get { return _childNorm_MinAge; }
            set { _childNorm_MinAge = value; }
        }

        private string _childNorm_MaxAge = "";
        /// <summary>
        /// 儿童标准-年龄最大
        /// </summary>
        public string ChildNorm_MaxAge
        {
            get { return _childNorm_MaxAge; }
            set { _childNorm_MaxAge = value; }
        }

        private string _childNorm_BedAge = "不占床";
        /// <summary>
        /// 儿童标准-年龄-床
        /// </summary>
        public string ChildNorm_BedAge
        {
            get { return _childNorm_BedAge; }
            set { _childNorm_BedAge = value; }
        }

        private string _childNorm_BedAgeExplain = "";
        /// <summary>
        /// 儿童标准-年龄-床描述
        /// </summary>
        public string ChildNorm_BedAgeExplain
        {
            get { return _childNorm_BedAgeExplain; }
            set { _childNorm_BedAgeExplain = value; }
        }

        private string _childNorm_MinHeight = "";
        /// <summary>
        /// 儿童标准-身高-最小
        /// </summary>
        public string ChildNorm_MinHeight
        {
            get { return _childNorm_MinHeight; }
            set { _childNorm_MinHeight = value; }
        }

        private string _childNorm_MaxHeight = "";
        /// <summary>
        /// 儿童标准-身高-最大
        /// </summary>
        public string ChildNorm_MaxHeight
        {
            get { return _childNorm_MaxHeight; }
            set { _childNorm_MaxHeight = value; }
        }

        private string _childNorm_BedHeight = "不占床";
        /// <summary>
        /// 儿童标准-身高-床
        /// </summary>
        public string ChildNorm_BedHeight
        {
            get { return _childNorm_BedHeight; }
            set { _childNorm_BedHeight = value; }
        }

        private string _childNorm_BedHeightExplain = "";
        /// <summary>
        /// 儿童标准-身高-床描述
        /// </summary>
        public string ChildNorm_BedHeightExplain
        {
            get { return _childNorm_BedHeightExplain; }
            set { _childNorm_BedHeightExplain = value; }
        }

        private string _childNorm_Others = "";
        /// <summary>
        /// 儿童标准-其他
        /// </summary>
        public string ChildNorm_Others
        {
            get { return _childNorm_Others; }
            set { _childNorm_Others = value; }
        }
        #endregion

        #region 其他
        private string _other_MultSelect = "";
        /// <summary>
        /// 其它-选择
        /// </summary>
        public string Other_MultSelect
        {
            get { return _other_MultSelect; }
            set { _other_MultSelect = value; }
        }

        private string _other_Desc = "";
        /// <summary>
        /// 其它-描述
        /// </summary>
        public string Other_Desc
        {
            get { return _other_Desc; }
            set { _other_Desc = value; }
        }
        #endregion

        #endregion

        /// <summary>
        /// 费用包含(简单)
        /// </summary>
        public string Fee_Include_Single { get; set; }

        /// <summary>
        /// 费用不包含
        /// </summary>
        public string Fee_NonClude { get; set; }

        /// <summary>
        /// 语种 1：中文，2英文
        /// </summary>
        public int ProductLang { get; set; }
    }

    /// <summary>
    /// 推荐活动
    /// </summary>
    public class ProductRecommendActivityDTO
    {
        /// <summary>
        /// 推荐活动Id
        /// </summary>
        public int RecommendActivityId { get; set; }

        /// <summary>
        /// 推荐活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 推荐活动描述
        /// </summary>
        public string ActivityDesc { get; set; }

        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal ReferencePrice { get; set; }
    }

    /// <summary>
    /// 动态属性
    /// </summary>
    public class ProductPropertyDTO
    {
        /// <summary>
        /// 产品属性Id
        /// </summary>
        public long ProductPropId { get; set; }

        /// <summary>
        /// 套餐Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 动态字段Key
        /// </summary>
        public string ElementKey { get; set; }

        /// <summary>
        /// 动态字段值
        /// </summary>
        public string ElementValue { get; set; }
    }

    /// <summary>
    /// 产品资源信息
    /// </summary>
    public class ProductResourceInfoDTO
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        public int ResourceTypeId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; } = string.Empty;

        /// <summary>
        /// 资源描述
        /// </summary>
        public string ResourceDesc { get; set; } = "";

        /// <summary>
        /// 是否可订（0：不可选；1：可选；2：必选）
        /// </summary>
        public int IsBookable { get; set; } = 1;
    }
    /// <summary>
    /// 套餐基本信息
    /// </summary>
    public class ItemBasicInfoDTO
    {
        /// <summary>
        /// 套餐Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string ItemName { get; set; }
    }
    public class CityDTO
    {
        /// <summary>
        /// 城市Id
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 是否已选
        /// </summary>
        public bool IsSelected { get; set; }
    }
    /// </summary>
    public class ItineraryDTO
    {
        /// <summary>
        /// 线路Id
        /// </summary>
        public int ItineraryId { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string ItineraryName { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 是否已选
        /// </summary>
        public bool IsSelected { get; set; }
    }
    /// <summary>
    /// 行程信息
    /// </summary>
    public class ProductSegmentDTO
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 行程Id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 行程名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 天数
        /// </summary>
        public int DayCount { get; set; }

        /// <summary>
        /// 晚数
        /// </summary>
        public int NightCount { get; set; }

        /// <summary>
        /// 去程交通
        /// </summary>
        public string GoBy { get; set; }

        /// <summary>
        /// 返程交通
        /// </summary>
        public string BackBy { get; set; }

        /// <summary>
        /// 推荐行程
        /// </summary>
        public string RecommendSegment { get; set; } = "";

        /// <summary>
        /// 行程附件
        /// </summary>
        public string SegmentUrl { get; set; }

        /// <summary>
        /// 行程信息
        /// </summary>
        public List<SegmentDTO> Segments { get; set; }

        /// <summary>
        /// 接客/送客地址
        /// </summary>
        public List<PickUp> DepartCarList { get; set; }
    }
    /// <summary>
    /// 行程
    /// </summary>
    public class SegmentDTO
    {
        /// <summary>
        /// 行程ID
        /// </summary>
        public long SegmentId { get; set; }

        /// <summary>
        /// 天数
        /// </summary>
        public int DayNo { get; set; }

        /// <summary>
        /// 行程名称（icon：航班：${planeicon}；火车：${motoricon}；巴士：${trainicon}；轮船：${shipicon}；汽车：${caricon}）
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 行程描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 行程明细
        /// </summary>
        public List<SegmentDetailDTO> SegmentDetails { get; set; }

        /// <summary>
        /// 酒店
        /// </summary>
        public HotelDTO Hotel { get; set; }

        /// <summary>
        /// 餐食
        /// </summary>
        public List<MealDTO> Meals { get; set; }
    }
    /// <summary>
    /// 行程明细
    /// </summary>
    public class SegmentDetailDTO
    {
        /// <summary>
        /// 行程明细ID
        /// </summary>
        public long SegmentDetailId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 类型（Breakfast：早餐/Lunch：午餐/Dinner：晚餐/SegmentDetail：行程）
        /// </summary>
        public string DetailType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string DetailDesc { get; set; }

        /// <summary>
        /// 景点列表
        /// </summary>
        public List<ProductResourceDTO> Resources { get; set; }
    }

    /// <summary>
    /// 接送点
    /// </summary>
    public class PickUp
    {
        /// <summary>
        /// 接送点（0：接；1：送；）
        /// </summary>
        public int TargetType { get; set; }

        /// <summary>
        /// 发车信息Id
        /// </summary>
        public int DepartCarId { get; set; }

        /// <summary>
        /// 发车时间（格式：HH:mm）
        /// </summary>
        public string DepartTime { get; set; }

        /// <summary>
        /// 接送点名称
        /// </summary>
        public string AddressName { get; set; }

        /// <summary>
        /// 发车地址
        /// </summary>
        public string DepartAddress { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    /// <summary>
    /// 住宿
    /// </summary>
    public class HotelDTO
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 酒店列表
        /// </summary>
        public List<ProductResourceDTO> Resources { get; set; }
    }

    /// <summary>
    /// 餐食
    /// </summary>
    public class MealDTO
    {
        /// <summary>
        /// 类型（Breakfast：早餐；Lunch：午餐；Dinner：晚餐）
        /// </summary>
        public string MealType { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 餐食描述
        /// </summary>
        public string MealDesc { get; set; }
    }

}
