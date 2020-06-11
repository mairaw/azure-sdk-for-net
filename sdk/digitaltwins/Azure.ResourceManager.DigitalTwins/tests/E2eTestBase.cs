// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Azure.Core.TestFramework;

namespace Azure.ResourceManager.DigitalTwins.Tests
{
    [ClientTestFixture]
    public abstract class E2eTestBase : RecordedTestBase<DigitalTwinsManagementTestEnvironment>
    {
        private static readonly TimeSpan s_pollingInterval = TimeSpan.FromSeconds(3);

        protected DigitalTwinsManagementClient Client { get; private set; }
        protected Random Rand { get; } = new Random();

        protected E2eTestBase(bool isAsync)
            : base(isAsync, RecordedTestMode.Live)
        {
        }

        protected E2eTestBase(bool isAsync, RecordedTestMode mode)
            : base(isAsync, mode)
        {
        }

        protected DigitalTwinsManagementClient InitClient()
        {
            Client = InstrumentClient(
                new DigitalTwinsManagementClient(
                TestEnvironment.SubscriptionId,
                TestEnvironment.Credential,
                Recording?.InstrumentClientOptions(new DigitalTwinsManagementClientOptions())));

            return Client;
        }

        protected ValueTask<Response<T>> WaitForCompletionAsync<T>(Operation<T> operation)
        {
            if (Mode == RecordedTestMode.Playback)
            {
                return operation.WaitForCompletionAsync(s_pollingInterval, default);
            }

            return operation.WaitForCompletionAsync();
        }
    }
}
