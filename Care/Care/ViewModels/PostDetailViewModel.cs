using Care.Models;
using Care.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Care.ViewModels
{
    [QueryProperty(nameof(PostId), nameof(PostId))]
    public class PostDetailViewModel : BaseViewModel
    {
        private string postId;
        private string orgName;
        private string orgLongDescr;
        private string orgLink;
        private string orgPhotoName;
        private string orgLogoName;

        private readonly PostService context;
        public int OrgId { get; set; }

        public PostModel Post { get; }

        public PostDetailViewModel()
        {
            this.context = new PostService();
            Title = "Organization";
        }

        public string OrgName
        {
            get => orgName;
            set => SetProperty(ref orgName, value);
        }

        public string OrgLongDescr
        {
            get => orgLongDescr;
            set => SetProperty(ref orgLongDescr, value);
        }

        public string OrgLink
        {
            get => orgLink;
            set => SetProperty(ref orgLink, value);
        }
        public string OrgPhotoName
        {
            get => orgPhotoName;
            set => SetProperty(ref orgPhotoName, value);
        }
        public string OrgLogoName
        {
            get => orgLogoName;
            set => SetProperty(ref orgLogoName, value);
        }

        public string PostId
        {
            get
            {
                return postId;
            }
            set
            {
                postId = value;
                LoadPostId(value);
            }
        }

        public void LoadPostId(string postId)
        {
            try
            {
                var post = context.FindById(Int32.Parse(postId));
                OrgId = post.OrgId;
                OrgName = post.OrgName;
                OrgLongDescr = post.OrgLongDescr;
                OrgLink = post.OrgLink;
                OrgPhotoName = post.OrgPhotoName;
                OrgLogoName = post.OrgLogoName;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Post");
            }
        }
    }
}
