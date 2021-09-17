using AutoMapper;

namespace Ngnet.Mapper
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
