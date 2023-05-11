// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#pragma once
#include <napi.h>
#include <memory>

#include "../oc_abi/orchestrator_abi.h"

using namespace std;
using namespace Napi;

namespace oc 
{
    class LabelResolverNode : public Napi::ObjectWrap<LabelResolverNode> {
    public:
        LabelResolverNode(const Napi::CallbackInfo& info);
        static Napi::Object Init(Napi::Env env, Napi::Object exports);
        static Napi::Object NewInstance(Napi::Env env, Napi::Value arg);

        ~LabelResolverNode();

    private:
        static Napi::FunctionReference constructor;

        Napi::Value AddSnapshot(const Napi::CallbackInfo& info);
        Napi::Value CreateSnapshot(const Napi::CallbackInfo& info);

        Napi::Value AddExample(const Napi::CallbackInfo& info);

        Napi::Value RemoveExample(const Napi::CallbackInfo& info);
        Napi::Value GetExamples(const Napi::CallbackInfo& info);

        Napi::Value GetLabels(const Napi::CallbackInfo& info);
        Napi::Value RemoveLabel(const Napi::CallbackInfo& info);
        Napi::Value Score(const Napi::CallbackInfo& info);

        Napi::Value GetConfigJson(const Napi::CallbackInfo& info);
        Napi::Value SetRuntimeParams(const Napi::CallbackInfo& info);

        Napi::Value AddBatch(const Napi::CallbackInfo& info);
        Napi::Value ScoreBatch(const Napi::CallbackInfo& info);
        
        string GetErrorMsg(oc_error_t err, resource_t resource = 0);

        label_resolver_t _resolver;
    };

    struct LabelResolverParams
    {
        orchestrator_t orchestrator; 
        vector<uint8_t> snapshot;
    };

} // namespace oc
