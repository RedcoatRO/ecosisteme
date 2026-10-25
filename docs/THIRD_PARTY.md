# External resources and licenses

No external art, audio, biological datasets or binary source dependencies are checked in. Scene primitives and application code are authored for this task. Beneficiary project license remains unspecified; no public license grant was invented. Dependency terms below are separate from product licensing. Retain notices/licenses when redistributing; verify current terms before shipping. Registry locks preserve NuGet content hashes. Sources were inspected for setup, not copied into application code.

| Resource / version | Author / source | License / commercial use and notices |
|---|---|---|
| .NET SDK10.0.402, reference assemblies2.1.0 | Microsoft/.NET Foundation; https://builds.dotnet.microsoft.com/dotnet/release-metadata/10.0/releases.json ; https://github.com/dotnet/dotnet | MIT for .NET code; retain license and bundled third-party notices. Exact SDK archive SHA512 is in tools/cloud-install.sh. No SDK binaries committed. |
| Microsoft.NET.Test.Sdk, TestPlatform.ObjectModel, TestPlatform.TestHost, CodeCoverage17.14.1 | Microsoft; https://github.com/microsoft/vstest | MIT per restored nuspec metadata; copyright/license retention. Test-only. |
| Newtonsoft.Json13.0.3 | James Newton-King; https://github.com/JamesNK/Newtonsoft.Json | MIT per restored nuspec; retain notice. Transitive test-only. |
| xunit2.9.3, assert/core/extensibility2.9.3, analyzers1.18.0 | James Newkirk, Brad Wilson, Marcin Budny; https://github.com/xunit/xunit | Apache-2.0 per restored nuspec; retain license/required notices and note modifications if redistributed. Test-only, no modifications here. |
| xunit.abstractions2.0.3 | James Newkirk, Brad Wilson; https://github.com/xunit/abstractions.xunit | Apache-2.0 project license; legacy nuspec points to xunit license URL. Retain bundled license; test-only. |
| xunit.runner.visualstudio3.1.0 | James Newkirk, Brad Wilson; https://github.com/xunit/visualstudio.xunit | Apache-2.0 per restored nuspec; test-only. |
| Unity6000.0.66f2 and builtin modules1.0.0 | Unity Technologies; https://unity.com/legal/terms-of-service/software | Proprietary Unity terms, not MIT; Personal can be no-cost only if eligibility/current terms permit. Eligibility/activation NOT VERIFIED. No editor redistribution. |
| URP17.0.4 and transitive Unity packages (not restored) | Unity Technologies; https://github.com/Unity-Technologies/Graphics/tree/feb4de2d9a93a4ae10d287d3a6d7003d08ea3e53 | Unity Companion License for Graphics code, used with Unity under its terms; retain package notices. Full transitive license inventory remains pending actual UPM resolution. Do not import other assets without explicit commercially compatible terms. |
| actions/checkout, commit11bd71901bbe5b1630ceea73d27597364c9af683 | GitHub; https://github.com/actions/checkout | MIT; CI example only. No service runtime dependency. |

Unity reference-source tag https://github.com/Unity-Technologies/UnityCsReference/tree/6000.0.66f2 was used as read-only API/version evidence. It is not an Editor binary or a commercially reusable asset imported into the product. Android SDK/JDK/NDK licenses and exact bundled versions must be inventoried after actual installation; none is claimed installed here.
