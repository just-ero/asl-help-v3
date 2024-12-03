using System.Collections.Generic;

using AslHelp.Api;
using AslHelp.Api.Requests;
using AslHelp.Api.Responses;
using AslHelp.Api.Servers;

namespace AslHelp.Native.Mono;

internal sealed class NativeMonoServer : MonoServerBase
{
    public NativeMonoServer()
        : base(ApiResourceStrings.PipeName) { }

    protected override unsafe GetMonoImageResponse GetMonoImage(GetMonoImageRequest request)
    {
        Output.Log($"[GetMonoImage] Request: {request.FileName}");

        nuint image = MonoApi.MonoImage_Loaded(request.FileName);
        if (image == 0)
        {
            Output.Log("[GetMonoImage]   => Failure!");
            return new(0, "", "", "");
        }

        Output.Log($"[GetMonoImage]   => Success: 0x{image:X}.");

        return new(
            image,
            MonoApi.MonoImage_GetName(image),
            MonoApi.MonoImage_GetName(image) + ".dll",
            MonoApi.MonoImage_GetFileName(image));
    }

    protected override unsafe GetMonoClassResponse GetMonoClass(GetMonoClassRequest request)
    {
        Output.Log($"[GetMonoClass] Request: {string.Join('.', request.Namespace, request.Name)}");

        nuint klass = MonoApi.MonoClass_FromName((nuint)request.Image, request.Namespace, request.Name);
        if (klass == 0)
        {
            Output.Log("[GetMonoClass]   => Failure!");
            return new(0);
        }

        Output.Log($"[GetMonoClass]   => Success: 0x{(ulong)klass:X}.");

        return new(
            klass);
    }

    protected override IEnumerable<GetMonoClassFieldResponse> GetMonoClassFields(GetMonoClassFieldsRequest request)
    {
        // Output.Log($"[GetMonoClassFields] Request: {request.Klass:X}");

        // nuint iter = 0;
        // while (true)
        // {
        //     ulong address;
        //     int offset;
        //     string name;
        //     string typeName;

        //     unsafe
        //     {
        //         Output.Log($"[GetMonoClassFields]   => Loading next field...");

        //         nuint field = MonoApi.mono_class_get_fields((MonoClass*)request.Klass, ref iter);
        //         if (field == 0)
        //         {
        //             Output.Log($"[GetMonoClassFields]     => No more fields!");
        //             break;
        //         }

        //         Output.Log($"[GetMonoClassFields]     => Success: 0x{(ulong)field:X}.");

        //         address = (ulong)field;
        //         offset = field->Offset;
        //         name = StringMarshal.CreateStringFromNullTerminated(field->Name);
        //         typeName = MonoApi.mono_type_get_name_full(field->Type, MonoTypeNameFormat.FullName);
        //     }

        //     yield return new(
        //         address,
        //         offset,
        //         name,
        //         typeName);
        // }

        yield break;
    }
}
