using System.Collections.Generic;

using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Api.Servers;
using AslHelp.Native.Mono.Metadata;
using AslHelp.Native.Utils;

namespace AslHelp.Native.Mono;

internal sealed class NativeMonoServer : MonoServerBase
{
    public NativeMonoServer()
        : base(ApiResourceStrings.PipeName) { }

    protected override unsafe GetMonoImageResponse GetMonoImage(GetMonoImageRequest request)
    {
        Trace.Log($"[GetMonoImage] Request: {request.FileName}");

        MonoImage* image = MonoApi.mono_image_loaded(request.FileName);
        if (image is null)
        {
            Trace.Log("[GetMonoImage]   => Failure!");
            return new(0, "", "", "");
        }

        Trace.Log($"[GetMonoImage]   => Success: 0x{(ulong)image:X}.");

        return new(
            (ulong)image,
            StringMarshal.CreateStringFromNullTerminated(image->AssemblyName),
            StringMarshal.CreateStringFromNullTerminated(image->ModuleName),
            StringMarshal.CreateStringFromNullTerminated(image->Name));
    }

    protected override unsafe GetMonoClassResponse GetMonoClass(GetMonoClassRequest request)
    {
        Trace.Log($"[GetMonoClass] Request: {string.Join('.', request.Namespace, request.Name)}");

        MonoClass* klass = MonoApi.mono_class_from_name_case((MonoImage*)request.Image, request.Namespace, request.Name);
        if (klass is null)
        {
            Trace.Log("[GetMonoClass]   => Failure!");
            return new(0);
        }

        Trace.Log($"[GetMonoClass]   => Success: 0x{(ulong)klass:X}.");

        return new(
            (ulong)klass);
    }

    protected override IEnumerable<GetMonoClassFieldResponse> GetMonoClassFields(GetMonoClassFieldsRequest request)
    {
        Trace.Log($"[GetMonoClassFields] Request: {request.Klass:X}");

        nuint iter = 0;
        while (true)
        {
            ulong address;
            int offset;
            string name;

            unsafe
            {
                Trace.Log($"[GetMonoClassFields]   => Loading next field...");

                MonoClassField* field = MonoApi.mono_class_get_fields((MonoClass*)request.Klass, ref iter);
                if (field is null)
                {
                    Trace.Log($"[GetMonoClassFields]     => No more fields!");
                    break;
                }

                Trace.Log($"[GetMonoClassFields]     => Success: 0x{(ulong)field:X}.");

                address = (ulong)field;
                offset = field->Offset;
                name = StringMarshal.CreateStringFromNullTerminated(field->Name);
            }

            yield return new(
                address,
                offset,
                name);
        }
    }
}
