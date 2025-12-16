namespace ZR.Model.System.Dto
{
    /// <summary>
    /// 文件存储输入对象
    /// </summary>
    public class SysFileDto
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }
        /// <summary>
        /// 文件原名
        /// </summary>
        [ExcelColumn(Name = "原文件名", Width = 15)]
        public string RealName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [ExcelColumn(Name = "文件类型")]
        public string FileType { get; set; }
        /// <summary>
        /// 存储文件名
        /// </summary>
        [ExcelColumn(Name = "存储文件名")]
        public string FileName { get; set; }
        /// <summary>
        /// 文件存储地址 eg：/uploads/20220202
        /// </summary>
        [ExcelColumn(Name = "文件存储地址", Width = 30)]
        public string FileUrl { get; set; }
        /// <summary>
        /// 仓库位置 eg：/uploads
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public string StorePath { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [ExcelColumn(Name = "文件大小")]
        public string FileSize { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [ExcelColumn(Name = "扩展名")]
        public string FileExt { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public string Create_by { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        [ExcelColumn(Name = "上传时间", Format = "yyyy-MM-dd")]
        public DateTime? Create_time { get; set; }
        /// <summary>
        /// 存储类型
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public int? StoreType { get; set; }
        /// <summary>
        /// 目录
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public int CategoryId { get; set; }
        [ExcelColumn(Ignore = true)]
        public string ClassifyType { get; set; }

        [ExcelColumn(Name = "组名")]
        public string GroupName { get; set; }
        /// <summary>
        /// 访问路径
        /// </summary>
        [ExcelColumn(Name = "访问路径", Width = 50)]
        public string AccessUrl { get; set; }
        public SysFileDto() { }
        public SysFileDto(string originFileName, string fileName, string ext, string fileSize, string storePath, string accessUrl, string create_by)
        {
            StorePath = storePath;
            RealName = originFileName;
            FileName = fileName;
            FileExt = ext;
            FileSize = fileSize;
            AccessUrl = accessUrl;
            Create_by = create_by;
            Create_time = DateTime.Now;
        }
    }
    public class SysFileQueryDto : PagerInfo
    {
        /// <summary>
        /// 分类
        /// </summary>
        public string ClassifyType { get; set; }
        public DateTime? BeginCreate_time { get; set; }
        public DateTime? EndCreate_time { get; set; }
        public int? StoreType { get; set; }
        public long? FileId { get; set; }
        public int? CategoryId { get; set; }
    }

    public class FileGroupMoveRequest
    {
        public int GroupId { get; set; }   // 或 int，根据你的数据库字段类型
        public List<long> Ids { get; set; } = [];
    }
}
