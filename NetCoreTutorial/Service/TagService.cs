using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreTutorial.Domain;
using NetCoreTutorial.Repository;

namespace NetCoreTutorial.Service
{
    public interface ITagService
    {
        Task<List<Tag>> GetAllByName(string[] requestTags);
    }

    public class TagService : ITagService
    {
        private readonly IUnitOfWork<GenericBaseEntityRepository<Tag>> _unitOfWork;

        public TagService(IUnitOfWork<GenericBaseEntityRepository<Tag>> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Tag>> GetAllByName(string[] tagNames)
        {
            var list = new List<Tag>();
            foreach (var tagName in tagNames)
            {
                var tagInDb = await _unitOfWork.Repository.GetSingle(x => x.Name == tagName);
                if (tagInDb != null)
                {
                    list.Add(tagInDb);
                }
                else
                {
                    var tag = new Tag {Name = tagName};
                    await _unitOfWork.Repository.Add(tag);
                    list.Add(tag);
                }
            }

            return list;
        }
    }
}