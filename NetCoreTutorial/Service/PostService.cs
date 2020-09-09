using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreTutorial.Domain;
using NetCoreTutorial.Helpers;
using NetCoreTutorial.Model;
using NetCoreTutorial.Repository;

namespace NetCoreTutorial.Service
{
    public interface IPostService
    {
        Task<Post> AddPost(AppUser appUser, PostEditRequest request);
        Task<Post> EditPost(AppUser appUser, PostEditRequest request);
        Task DeletePost(AppUser appUser, long id);
        Task<IEnumerable<Post>> GetPosts();
    }

    public class PostService : IPostService
    {
        private readonly IUnitOfWork<GenericBaseEntityRepository<Post>> _unitOfWork;
        private readonly ITagService _tagService;
        private readonly IPostTagService _postTagService;

        public PostService(IUnitOfWork<GenericBaseEntityRepository<Post>> unitOfWork, ITagService tagService,
            IPostTagService postTagService)
        {
            _unitOfWork = unitOfWork;
            _tagService = tagService;
            _postTagService = postTagService;
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _unitOfWork.Repository.GetAll(
                include: x => x.Include(y => y.PostTags).ThenInclude(y => y.Tag),
                orderBy: x => x.OrderByDescending(y => y.Id));
        }

        public async Task<Post> AddPost(AppUser appUser, PostEditRequest request)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                UserId = appUser.Id
            };

            await UpdatePostTags(post, request);

            await _unitOfWork.Repository.Add(post);
            await _unitOfWork.SaveChanges();
            return post;
        }

        public async Task<Post> EditPost(AppUser appUser, PostEditRequest request)
        {
            if (!request.Id.HasValue)
            {
                throw new AppException("Id is not null");
            }

            var post = await _unitOfWork.Repository.GetSingle(x => x.Id == request.Id.Value);
            if (post == null)
            {
                throw new AppException("Post not found!");
            }

            post.Title = request.Title;
            post.Content = request.Content;
            post.UserId = appUser.Id;

            await UpdatePostTags(post, request);

            _unitOfWork.Repository.Update(post);
            await _unitOfWork.SaveChanges();
            return post;
        }

        private async Task UpdatePostTags(Post post, PostEditRequest request)
        {
            var tagList = new List<Tag>();

            if (request.Tags != null)
            {
                tagList = await _tagService.GetAllByName(request.Tags);
            }

            await _postTagService.UpdatePostTags(post, tagList);
        }

        public async Task DeletePost(AppUser appUser, long id)
        {
            var post = await _unitOfWork.Repository.GetSingle(x => x.Id == id);
            _unitOfWork.Repository.Remove(post);
            await _unitOfWork.SaveChanges();
        }
    }
}