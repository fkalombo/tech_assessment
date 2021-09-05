using System.Collections.Generic;

namespace coding_project_v2.json_classes
{
    class Service
    {
        public string baseUrl { get; set; }
        public bool enabled { get; set; }
        public IList<Endpoint> endpoints { get; set; }
    }
}
