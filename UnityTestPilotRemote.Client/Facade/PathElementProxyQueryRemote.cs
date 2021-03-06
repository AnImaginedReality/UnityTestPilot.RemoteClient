﻿using AIR.UnityTestPilot.Interactions;
using AIR.UnityTestPilot.Queries;
using AIR.UnityTestPilotRemote.Common;

namespace AIR.UnityTestPilotRemote.Client
{
    public class PathElementProxyQueryRemote : PathElementQuery, IProxyQuery
    {
        public readonly bool IsActive;
        public readonly string Name;
        public readonly string Text;
        private UnityDriverHostProcess _process;

        private PathElementProxyQueryRemote(RemoteUiElement response)
            : base(response.Name)
        {
            IsActive = response.IsActive;
            Name = response.Name;
            Text = response.Text;
        }

        public PathElementProxyQueryRemote(string name)
            : base(name) => Name = name;

        public void ProxyVia(UnityDriverHostProcess process)
            => _process = process;

        public override UiElement[] Search()
        {
            // Convert to RemoteQuery.
            var remoteElementQuery = new RemoteElementQuery(
                QueryFormat.PathQuery,
                PathToFind,
                string.Empty
            );

            // Submit
            var remoteElementQueryTask = _process
                .Query(remoteElementQuery);

            // Get result
            var result = remoteElementQueryTask.Result;

            // Convert  RemoteUiElement to UiElement
            var resultElements = new[] {
                new UiElementRemote(result, _process) };

            // Return that
            return resultElements;
        }

    }
}