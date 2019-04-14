﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static LiteDB.Constants;

namespace LiteDB.Engine
{
    public partial class LiteEngine
    {
        private IEnumerable<BsonDocument> SysDatabase()
        {
            var doc = new BsonDocument();

            doc["name"] = _disk.GetName(FileOrigin.Data);    
            doc["limitSize"] = (int)_settings.LimitSize;
            doc["timeout"] = _settings.Timeout.TotalSeconds;
            doc["utcDate"] = _settings.UtcDate;
            doc["readOnly"] = _settings.ReadOnly;
            doc["checkpointOnShutdown"] = _settings.CheckpointOnShutdown;

            doc["lastPageID"] = (int)_header.LastPageID;
            doc["freeEmptyPageID"] = (int)_header.FreeEmptyPageID;

            doc["creationTime"] = _header.CreationTime;
            doc["lastCheckpoint"] = _header.LastCheckpoint;

            doc["dataFileSize"] = (int)_disk.GetLength(FileOrigin.Data);
            doc["logFileSize"] = (int)_disk.GetLength(FileOrigin.Log);
            doc["asyncQueueLength"] = _disk.Queue.Length;

            doc["userVersion"] = _header.UserVersion;

            doc["cache"] = new BsonDocument
            {
                ["extendSegments"] = _disk.Cache.ExtendSegments,
                ["memoryUsage"] = 
                    (_disk.Cache.ExtendSegments * MEMORY_SEGMENT_SIZE * PAGE_SIZE) +
                    (40 * (_disk.Cache.ExtendSegments * MEMORY_SEGMENT_SIZE)),
                ["freePages"] = _disk.Cache.FreePages,
                ["pagesInUse"] = _disk.Cache.PagesInUse,
                ["unusedPages"] = _disk.Cache.UnusedPages
            };

            yield return doc;
        }
    }
}