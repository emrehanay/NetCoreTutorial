using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreTutorial.Domain;
using NetCoreTutorial.Repository;

namespace NetCoreTutorial.Service
{
    public interface IPostTagService
    {
        Task UpdatePostTags(Post post, List<Tag> tags);
    }

    public class PostTagService : IPostTagService
    {
        private readonly IUnitOfWork<GenericEntityRepository<PostTag>> _unitOfWork;

        public PostTagService(IUnitOfWork<GenericEntityRepository<PostTag>> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdatePostTags(Post post, List<Tag> tags)
        {
            if (post.Id != 0)
            {
                var postTagsInDb =
                    await _unitOfWork.Repository.GetAll(x =>
                        x.PostId == post.Id);

                foreach (var postTag in postTagsInDb)
                {
                    _unitOfWork.Repository.Remove(postTag);
                }
            }

            foreach (var tag in tags)
            {
                var postTag = new PostTag
                {
                    Post = post,
                    Tag = tag
                };
                await _unitOfWork.Repository.Add(postTag);
            }
        }
    }
}