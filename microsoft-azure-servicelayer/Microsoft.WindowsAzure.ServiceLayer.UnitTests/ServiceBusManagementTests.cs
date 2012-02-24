﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.ServiceLayer.ServiceBus;
using Windows.Foundation;

using Xunit;

namespace Microsoft.WindowsAzure.ServiceLayer.UnitTests
{
    /// <summary>
    /// Tests for the service bus management.
    /// </summary>
    public class ServiceBusManagementTests
    {
        /// <summary>
        /// Comparer for the QueueInfo type.
        /// </summary>
        private class InternalQueueInfoComparer : IEqualityComparer<QueueInfo>
        {
            bool IEqualityComparer<QueueInfo>.Equals(QueueInfo x, QueueInfo y)
            {
                return x.DefaultMessageTimeToLive == y.DefaultMessageTimeToLive
                    && x.DuplicateDetectionHistoryTimeWindow == y.DuplicateDetectionHistoryTimeWindow
                    && x.EnableBatchedOperations == y.EnableBatchedOperations
                    && x.EnableDeadLetteringOnMessageExpiration == y.EnableDeadLetteringOnMessageExpiration
                    && x.LockDuration == y.LockDuration
                    && x.MaxDeliveryCount == y.MaxDeliveryCount
                    && x.MaxSizeInMegabytes == y.MaxSizeInMegabytes
                    && x.MessageCount == y.MessageCount
                    && x.RequiresDuplicateDetection == y.RequiresDuplicateDetection
                    && x.RequiresSession == y.RequiresSession
                    && x.SizeInBytes == y.SizeInBytes;
            }

            int IEqualityComparer<QueueInfo>.GetHashCode(QueueInfo obj)
            {
                return obj.GetHashCode();
            }
        }

        /// <summary>
        /// Comparer for the TopicInfo type.
        /// </summary>
        private class InternalTopicInfoComparer : IEqualityComparer<TopicInfo>
        {
            bool IEqualityComparer<TopicInfo>.Equals(TopicInfo x, TopicInfo y)
            {
                return x.DefaultMessageTimeToLive == y.DefaultMessageTimeToLive
                    && x.DuplicateDetectionHistoryTimeWindow == y.DuplicateDetectionHistoryTimeWindow
                    && x.EnableBatchedOperations == y.EnableBatchedOperations
                    && x.MaxSizeInMegabytes == y.MaxSizeInMegabytes
                    && x.RequiresDuplicateDetection == y.RequiresDuplicateDetection
                    && x.SizeInBytes == y.SizeInBytes;
            }

            int IEqualityComparer<TopicInfo>.GetHashCode(TopicInfo obj)
            {
                return obj.GetHashCode();
            }
        }

        /// <summary>
        /// Comparer for the SubscriptionInfo type.
        /// </summary>
        private class InternalSubscriptionInfoComparer : IEqualityComparer<SubscriptionInfo>
        {
            bool IEqualityComparer<SubscriptionInfo>.Equals(SubscriptionInfo x, SubscriptionInfo y)
            {
                return
                    x.LockDuration == y.LockDuration
                    && x.MaxDeliveryCount == y.MaxDeliveryCount
                    && x.MessageCount == y.MessageCount
                    && x.RequiresSession == y.RequiresSession;
            }

            int IEqualityComparer<SubscriptionInfo>.GetHashCode(SubscriptionInfo obj)
            {
                return obj.GetHashCode();
            }
        }

        IServiceBusService Service { get; set; }
        IEqualityComparer<QueueInfo> QueueInfoComparer { get; set; }
        IEqualityComparer<TopicInfo> TopicInfoComparer { get; set; }
        IEqualityComparer<SubscriptionInfo> SubscriptionInfoComparer { get; set; }

        public ServiceBusManagementTests()
        {
            Service = ServiceBusService.Create(Configuration.ServiceNamespace, Configuration.UserName, Configuration.Password);
            QueueInfoComparer = new InternalQueueInfoComparer();
            TopicInfoComparer = new InternalTopicInfoComparer();
            SubscriptionInfoComparer = new InternalSubscriptionInfoComparer();
        }


        string GetUniqueEntityName()
        {
            return string.Format("UnitTests.{0}", Guid.NewGuid().ToString());
        }

        Dictionary<string, T> GetItems<T>(
            Func<IAsyncOperation<IEnumerable<T>>> getItems,
            Func<T, string> getName)
        {
            Dictionary<string, T> items = new Dictionary<string,T>(StringComparer.OrdinalIgnoreCase);

            foreach (T item in getItems().AsTask<IEnumerable<T>>().Result)
            {
                string itemName = getName(item);
                items.Add(itemName, item);
            }

            return items;
        }

        /// <summary>
        /// Tests null arguments in queue management API.
        /// </summary>
        [Fact]
        public void NullArgsInQueues()
        {
            Assert.Throws<ArgumentNullException>(() => Service.CreateQueueAsync(null));
            Assert.Throws<ArgumentNullException>(() => Service.CreateQueueAsync(null, new QueueSettings()));
            Assert.Throws<ArgumentNullException>(() => Service.CreateQueueAsync("foo", null));
            Assert.Throws<ArgumentNullException>(() => Service.GetQueueAsync(null));
            Assert.Throws<ArgumentNullException>(() => Service.DeleteQueueAsync(null));
        }

        /// <summary>
        /// Tests full lifecycle of a queue.
        /// </summary>
        [Fact]
        public void QueueLifecycle()
        {
            // Create a queue.
            string queueName = GetUniqueEntityName();
            QueueInfo newQueue = Service.CreateQueueAsync(queueName).AsTask<QueueInfo>().Result;

            // Confirm that the queue can be obtained from the server
            QueueInfo storedQueue = Service.GetQueueAsync(queueName).AsTask<QueueInfo>().Result;
            Assert.Equal<QueueInfo>(storedQueue, newQueue, QueueInfoComparer);

            // Confirm that the queue can be obtained in the list
            Dictionary<string, QueueInfo> queues = GetItems(
                () => { return Service.ListQueuesAsync(); },
                (queue) => { return queue.Name; });

            Assert.True(queues.ContainsKey(queueName));
            Assert.Equal<QueueInfo>(newQueue, queues[queueName], QueueInfoComparer);

            // Delete the queue
            Service.DeleteQueueAsync(queueName).AsTask().Wait();
            queues = GetItems(
                () => { return Service.ListQueuesAsync(); },
                (queue) => { return queue.Name; });

            Assert.False(queues.ContainsKey(queueName));
        }

        /// <summary>
        /// Verifies that using an existing name for a new queue result in an exception.
        /// </summary>
        [Fact]
        public void CreateQueueDuplicateName()
        {
            // Create a queue
            string queueName = GetUniqueEntityName();
            QueueInfo newQueue = Service.CreateQueueAsync(queueName).AsTask<QueueInfo>().Result;

            Task t = Service.CreateQueueAsync(queueName).AsTask();
            Assert.Throws<AggregateException>(() => t.Wait());

            Service.DeleteQueueAsync(queueName).AsTask().Wait();
        }

        /// <summary>
        /// Tests getting a missing queue.
        /// </summary>
        [Fact]
        public void GetMissingQueue()
        {
            string queueName = GetUniqueEntityName();
            Task t = Service.GetQueueAsync(queueName).AsTask();
            Assert.Throws<AggregateException>(() => t.Wait());
        }

        /// <summary>
        /// Tests deleting a missing queue.
        /// </summary>
        [Fact]
        public void DeleteMissingQueue()
        {
            string queueName = GetUniqueEntityName();
            Task t = Service.DeleteQueueAsync(queueName).AsTask();
            Assert.Throws<AggregateException>(() => t.Wait());
        }

        /// <summary>
        /// Verifies creation of the queue with all non-default parameters
        /// </summary>
        [Fact]
        public void CreateQueueWithNonDefaultParams()
        {
            string queueName = GetUniqueEntityName();
            QueueSettings settings = new QueueSettings();

            settings.DefaultMessageTimeToLive = TimeSpan.FromHours(24);
            settings.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromDays(2);
            settings.EnableBatchedOperations = false;
            settings.EnableDeadLetteringOnMessageExpiration = true;
            settings.LockDuration = TimeSpan.FromMinutes(3);
            settings.MaxDeliveryCount = 5;
            settings.MaxSizeInMegabytes = 2048;
            settings.RequiresDuplicateDetection = true;
            settings.RequiresSession = true;

            QueueInfo queueInfo = Service.CreateQueueAsync(queueName, settings).AsTask<QueueInfo>().Result;
            Assert.Equal(queueInfo.DefaultMessageTimeToLive, settings.DefaultMessageTimeToLive.Value);
            Assert.Equal(queueInfo.DuplicateDetectionHistoryTimeWindow, settings.DuplicateDetectionHistoryTimeWindow.Value);
            Assert.Equal(queueInfo.EnableBatchedOperations, settings.EnableBatchedOperations.Value);
            Assert.Equal(queueInfo.EnableDeadLetteringOnMessageExpiration, settings.EnableDeadLetteringOnMessageExpiration.Value);
            Assert.Equal(queueInfo.LockDuration, settings.LockDuration.Value);
            Assert.Equal(queueInfo.MaxDeliveryCount, settings.MaxDeliveryCount.Value);
            Assert.Equal(queueInfo.MaxSizeInMegabytes, settings.MaxSizeInMegabytes.Value);
            Assert.Equal(queueInfo.RequiresDuplicateDetection, settings.RequiresDuplicateDetection.Value);
            Assert.Equal(queueInfo.RequiresSession, settings.RequiresSession.Value);
        }

        /// <summary>
        /// Tests throwing ArgumentNullException for topic management API.
        /// </summary>
        [Fact]
        public void NullArgsInTopics()
        {
            Assert.Throws<ArgumentNullException>(() => Service.CreateTopicAsync(null));
            Assert.Throws<ArgumentNullException>(() => Service.CreateTopicAsync(null, new TopicSettings()));
            Assert.Throws<ArgumentNullException>(() => Service.CreateTopicAsync("foo", null));
            Assert.Throws<ArgumentNullException>(() => Service.GetTopicAsync(null));
            Assert.Throws<ArgumentNullException>(() => Service.DeleteTopicAsync(null));
        }

        /// <summary>
        /// Tests the complete lifecycle of a topic.
        /// </summary>
        [Fact]
        public void TopicLifecycle()
        {
            // Create a topic
            string topicName = GetUniqueEntityName();
            TopicInfo newTopic = Service.CreateTopicAsync(topicName).AsTask<TopicInfo>().Result;

            // Confirm that the topic can be obtained from the server.
            TopicInfo storedTopic = Service.GetTopicAsync(topicName).AsTask<TopicInfo>().Result;
            Assert.Equal<TopicInfo>(storedTopic, newTopic, TopicInfoComparer);

            // Conmfirm that the topic can be obtained in the list.
            Dictionary<string, TopicInfo> topics = GetItems(
                () => { return Service.ListTopicsAsync(); },
                (t) => { return t.Name; });
            Assert.True(topics.ContainsKey(topicName));
            Assert.Equal(newTopic, topics[topicName], TopicInfoComparer);

            // Delete the topic.
            Service.DeleteTopicAsync(topicName).AsTask().Wait();
            topics = GetItems(
                () => { return Service.ListTopicsAsync(); },
                (t) => { return t.Name; });

            Assert.False(topics.ContainsKey(topicName));
        }

        /// <summary>
        /// Tests creating two topics with identical names.
        /// </summary>
        [Fact]
        public void CreateTopicDuplicateName()
        {
            // Create a topic
            string topicName = GetUniqueEntityName();
            TopicInfo newTopic = Service.CreateTopicAsync(topicName).AsTask<TopicInfo>().Result;

            Task t = Service.CreateTopicAsync(topicName).AsTask();
            Assert.Throws<AggregateException>(() => t.Wait());

            Service.DeleteTopicAsync(topicName).AsTask().Wait();
        }

        /// <summary>
        /// Tests getting a missing topic.
        /// </summary>
        [Fact]
        public void GetMissingTopic()
        {
            string topicName = GetUniqueEntityName();
            Task t = Service.GetTopicAsync(topicName).AsTask() ;
            Assert.Throws<AggregateException>(() => t.Wait());
        }

        /// <summary>
        /// Tests deleting a missing topic.
        /// </summary>
        [Fact]
        public void DeleteMissingTopic()
        {
            string topicName = GetUniqueEntityName();
            Task t = Service.DeleteTopicAsync(topicName).AsTask();
            Assert.Throws<AggregateException>(() => t.Wait());
        }

        /// <summary>
        /// Tests creation of a topic with all non-default parameters.
        /// </summary>
        [Fact]
        public void CreateTopicWithNonDefaultParams()
        {
            string topicName = GetUniqueEntityName();
            TopicSettings settings = new TopicSettings();

            settings.DefaultMessageTimeToLive = TimeSpan.FromHours(24);
            settings.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromDays(2);
            settings.EnableBatchedOperations = false;
            settings.MaxSizeInMegabytes = 2048;
            settings.RequiresDuplicateDetection = true;

            TopicInfo topic = Service.CreateTopicAsync(topicName, settings).AsTask<TopicInfo>().Result;
            Assert.Equal(settings.DefaultMessageTimeToLive.Value, topic.DefaultMessageTimeToLive);
            Assert.Equal(settings.DuplicateDetectionHistoryTimeWindow.Value, topic.DuplicateDetectionHistoryTimeWindow);
            Assert.Equal(settings.EnableBatchedOperations.Value, topic.EnableBatchedOperations);
            Assert.Equal(settings.MaxSizeInMegabytes.Value, topic.MaxSizeInMegabytes);
            Assert.Equal(settings.RequiresDuplicateDetection.Value, topic.RequiresDuplicateDetection);
        }

        /// <summary>
        /// Tests ArgumentNullException for subscription parameters.
        /// </summary>
        [Fact]
        public void NullArgsInSubscriptions()
        {
            Assert.Throws<ArgumentNullException>(() => Service.ListSubscriptionsAsync(null));
            Assert.Throws<ArgumentNullException>(() => Service.CreateSubscriptionAsync(null, "test"));
            Assert.Throws<ArgumentNullException>(() => Service.CreateSubscriptionAsync("test", null));
            Assert.Throws<ArgumentNullException>(() => Service.CreateSubscriptionAsync(null, "test", new SubscriptionSettings()));
            Assert.Throws<ArgumentNullException>(() => Service.CreateSubscriptionAsync("test", null, new SubscriptionSettings()));
            Assert.Throws<ArgumentNullException>(() => Service.CreateSubscriptionAsync("test", "test", null));
            Assert.Throws<ArgumentNullException>(() => Service.GetSubscriptionAsync(null, "test"));
            Assert.Throws<ArgumentNullException>(() => Service.GetSubscriptionAsync("test", null));
            Assert.Throws<ArgumentNullException>(() => Service.DeleteSubscriptionAsync(null, "test"));
            Assert.Throws<ArgumentNullException>(() => Service.DeleteSubscriptionAsync("test", null));
        }

        /// <summary>
        /// Tests the complete lifetime of a subscription.
        /// </summary>
        [Fact]
        public void SubscriptionLifecycle()
        {
            string topicName = GetUniqueEntityName();
            string subscriptionName = GetUniqueEntityName();

            Service.CreateTopicAsync(topicName).AsTask().Wait();
            try
            {
                // Create a subscription.
                SubscriptionInfo newSubscription = Service.CreateSubscriptionAsync(topicName, subscriptionName).AsTask().Result;

                // Confirm that the subscription can be obtained from the server.
                SubscriptionInfo storedSubscription = Service.GetSubscriptionAsync(topicName, subscriptionName).AsTask().Result;
                Assert.Equal(storedSubscription, newSubscription, SubscriptionInfoComparer);

                // Confirm that the subscription appears in the list.
                Dictionary<string, SubscriptionInfo> subscriptions = GetItems(
                    () => { return Service.ListSubscriptionsAsync(topicName); },
                    (s) => { return s.Name; });
                Assert.True(subscriptions.ContainsKey(subscriptionName));
                Assert.Equal(newSubscription, subscriptions[subscriptionName], SubscriptionInfoComparer);

                // Delete the subscription.
                Service.DeleteSubscriptionAsync(topicName, subscriptionName).AsTask().Wait();
                subscriptions = GetItems(
                    () => { return Service.ListSubscriptionsAsync(topicName); },
                    (s) => { return s.Name; });
                
                Assert.False(subscriptions.ContainsKey(subscriptionName));
            }
            finally
            {
                Service.DeleteTopicAsync(topicName).AsTask().Wait();
            }
        }

        /// <summary>
        /// Tests creating two subscriptions with identical names.
        /// </summary>
        [Fact]
        public void CreateSubscriptionDuplicateName()
        {
            string topicName = GetUniqueEntityName();
            string subscriptionName = GetUniqueEntityName();

            Service.CreateTopicAsync(topicName).AsTask().Wait();
            try
            {
                Service.CreateSubscriptionAsync(topicName, subscriptionName).AsTask().Wait();
                Task<SubscriptionInfo> task = Service.CreateSubscriptionAsync(topicName, subscriptionName).AsTask();
                Assert.Throws<AggregateException>(() => task.Wait());
            }
            finally
            {
                Service.DeleteTopicAsync(topicName).AsTask().Wait();
            }
        }

        /// <summary>
        /// Tests getting a non-existing subscription from an existing topic.
        /// </summary>
        [Fact]
        public void GetMissingSubscription()
        {
            string topicName = GetUniqueEntityName();
            string subscriptionName = GetUniqueEntityName();

            Service.CreateTopicAsync(topicName).AsTask().Wait();
            try
            {
                Task<SubscriptionInfo> task = Service.GetSubscriptionAsync(topicName, subscriptionName).AsTask();
                Assert.Throws<AggregateException>(() => task.Wait());
            }
            finally
            {
                Service.DeleteTopicAsync(topicName).AsTask().Wait();
            }
        }

        /// <summary>
        /// Tests getting a subscription from a non-existing topic.
        /// </summary>
        [Fact]
        public void GetSubscriptionFromMissingTopic()
        {
            string topicName = GetUniqueEntityName();
            string subscriptionName = GetUniqueEntityName();

            Task<SubscriptionInfo> task = Service.GetSubscriptionAsync(topicName, subscriptionName).AsTask();
            Assert.Throws<AggregateException>(() => task.Wait());
        }

        /// <summary>
        /// Tests getting all subscriptions from a non-existing topic.
        /// </summary>
        [Fact]
        public void ListSubscriptionsInMissingTopic()
        {
            string topicName = GetUniqueEntityName();

            List<SubscriptionInfo> subscriptions = new List<SubscriptionInfo>(
                Service.ListSubscriptionsAsync(topicName).AsTask<IEnumerable<SubscriptionInfo>>().Result);
            Assert.Equal(subscriptions.Count, 0);
        }

        /// <summary>
        /// Tests deleting a non-existing subscription.
        /// </summary>
        [Fact]
        public void DeleteMissingSubscription()
        {
            string topicName = GetUniqueEntityName();
            string subscriptionName = GetUniqueEntityName();

            Service.CreateTopicAsync(topicName).AsTask().Wait();
            try
            {
                Task task = Service.DeleteSubscriptionAsync(topicName, subscriptionName).AsTask();
                Assert.Throws<AggregateException>(() => task.Wait());
            }
            finally
            {
                Service.DeleteTopicAsync(topicName).AsTask().Wait();
            }
        }
    }
}
