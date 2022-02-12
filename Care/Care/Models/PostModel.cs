using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections;
using Care.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Plugin.Media.Abstractions;

namespace Care.Models
{
    public class PostModel : IEquatable<PostModel>, IComparable<PostModel>
    {
        [Required]
        [Key]
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string OrgShortDescr { get; set; }
        public string OrgLongDescr { get; set; }
        public string OrgLink { get; set; }

        public string OrgLogoName { get; set; }
        [NotMapped]
        public MediaFile OrgLogoFile { get; set; }

        public string OrgPhotoName { get; set; }
        [NotMapped]
        public MediaFile OrgPhotoFile { get; set; }

        public PostModel(int OrgId, string OrgName, string OrgShortDescr, string OrgLongDescr, string OrgLink, string OrgLogoName, string OrgPhotoName)
        {
            this.OrgId = OrgId;
            this.OrgName = OrgName;
            this.OrgShortDescr = OrgShortDescr;
            this.OrgLongDescr = OrgLongDescr;
            this.OrgLink = OrgLink;
            this.OrgLogoName = OrgLogoName;
            this.OrgPhotoName = OrgPhotoName;
        }

        public PostModel()
        {
        }

        public override bool Equals(object obj) =>
        (obj is PostModel post)
                ? Equals(post)
                : false;

        public int SortByNameAscending(string name1, string name2) => name1?.CompareTo(name2) ?? 1;

        public int CompareTo(PostModel comparePost) => comparePost == null ? 1 : OrgName.CompareTo(comparePost.OrgName);

        public bool Equals(PostModel other) => other is null ? false : OrgName.Equals(other.OrgName);

    }

    public class Posts<T> : IEnumerable<T>
    {
        private readonly T[] array;
        
        public Posts(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            this.array = array;
        }

        public int IndexOf(IEnumerable<T> source, T value)
        {
            int index = 0;
            var comparer = EqualityComparer<T>.Default; 
            foreach (T item in source)
            {
                if (comparer.Equals(item, value)) return index;
                index++;
            }
            return -1;
        }

        public IEnumerator<T> GetEnumerator() => new PostsEnum<T>(array);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
        public class PostsEnum<T> : IEnumerator<T>
        {
            private readonly T[] array;

            public PostsEnum(T[] array)
            {
                this.array = array;
            }

            private int index = -1;

            public T Current => index >= 0 && index < array.Length ? array[index] : default(T);

            object IEnumerator.Current => Current;

        public bool MoveNext()
            {
                index++;
                return index < array.Length;
            }

            public void Reset()
            {
                index = -1;
            }

            public void Dispose()
            { }
    }

}
