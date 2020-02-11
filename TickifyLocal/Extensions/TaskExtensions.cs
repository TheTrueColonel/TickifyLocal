using System;
using System.Threading.Tasks;
using Discord;

namespace Tickify.Extensions {
    public static class TaskExtensions {
        public static Task DeleteAfterTimeSpan (this IDeletable message, TimeSpan timeSpan) {
            return Task.Delay(timeSpan).ContinueWith(async _ => await message?.DeleteAsync());
        }

        public static Task DeleteAfterSeconds<T> (this Task<T> task, double seconds, bool awaitDeletion = false) where T : IDeletable => 
            task?.DeleteAfterTimeSpan(TimeSpan.FromSeconds(seconds), awaitDeletion);

        public static Task DeleteAfterTimeSpan<T> (this Task<T> task, TimeSpan timeSpan, bool awaitDeletion = false) where T : IDeletable {
            var deletion = Task.Run(async () => await (await task)?.DeleteAfterTimeSpan(timeSpan));
            return awaitDeletion ? deletion : task;
        }
    }
}