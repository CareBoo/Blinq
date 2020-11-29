# [3.1.0-preview.1](https://github.com/CareBoo/Blinq/compare/v3.0.0...v3.1.0-preview.1) (2020-11-29)


### Features

* :sparkles: Implement RunSingle and ScheduleSingle ([92366db](https://github.com/CareBoo/Blinq/commit/92366db433146f01d93dd161a7e210b02b5704a7)), closes [#78](https://github.com/CareBoo/Blinq/issues/78)

# [3.0.0](https://github.com/CareBoo/Blinq/compare/v2.0.0...v3.0.0) (2020-11-28)


### Bug Fixes

* :ambulance: Fix some enumerators that weren't compatible with Burst ([e215d09](https://github.com/CareBoo/Blinq/commit/e215d0907539b9e2b25cff0175c77be34370b16a)), closes [#84](https://github.com/CareBoo/Blinq/issues/84)


### Features

* :sparkles: Implement RunAverage and ScheduleAverage ([9f858a9](https://github.com/CareBoo/Blinq/commit/9f858a924c04af3281f5fc196fbcc3280789c75e)), closes [#64](https://github.com/CareBoo/Blinq/issues/64)


### BREAKING CHANGES

* Completely changed the ValueSequence API to include TEnumerator generic parameter

# [3.0.0-preview.1](https://github.com/CareBoo/Blinq/compare/v2.1.0-preview.1...v3.0.0-preview.1) (2020-11-27)


### Bug Fixes

* :ambulance: Fix some enumerators that weren't compatible with Burst ([2604908](https://github.com/CareBoo/Blinq/commit/2604908608143658a42b8c9febb5849c50e7dbaa)), closes [#84](https://github.com/CareBoo/Blinq/issues/84)


### BREAKING CHANGES

* Completely changed the ValueSequence API to include TEnumerator generic parameter

# [2.1.0-preview.1](https://github.com/CareBoo/Blinq/compare/v2.0.0...v2.1.0-preview.1) (2020-11-21)


### Features

* :sparkles: Implement RunAverage and ScheduleAverage ([fb94dc6](https://github.com/CareBoo/Blinq/commit/fb94dc6b3c76a2cea204d61ec26d8637bfe3f71e)), closes [#64](https://github.com/CareBoo/Blinq/issues/64)

# [2.0.0](https://github.com/CareBoo/Blinq/compare/v1.0.0...v2.0.0) (2020-11-20)


### Features

* :sparkles: Add `IGrouping` GroupBy implementation ([df7c623](https://github.com/CareBoo/Blinq/commit/df7c62342fc610c96910b157e2b529b43027f80a)), closes [#56](https://github.com/CareBoo/Blinq/issues/56)
* :sparkles: Add allocator API ([a8773d3](https://github.com/CareBoo/Blinq/commit/a8773d3a1df47fd4131000ef10e0ce7046ae5e30))
* :sparkles: Use `Burst.Delegates` ValueFuncs ([61e4fe1](https://github.com/CareBoo/Blinq/commit/61e4fe16a58f43ff910aaf4327195cae626e42f8)), closes [#55](https://github.com/CareBoo/Blinq/issues/55)


### BREAKING CHANGES

* `Execute` API is removed in ValueSequence
* ValueFunc API is now implemented by a package

# [2.0.0-preview.3](https://github.com/CareBoo/Blinq/compare/v2.0.0-preview.2...v2.0.0-preview.3) (2020-11-20)


### Features

* :sparkles: Add `IGrouping` GroupBy implementation ([a5a7cb8](https://github.com/CareBoo/Blinq/commit/a5a7cb816a54129b422162e990e7421acb3da405)), closes [#56](https://github.com/CareBoo/Blinq/issues/56)

# [2.0.0-preview.2](https://github.com/CareBoo/Blinq/compare/v2.0.0-preview.1...v2.0.0-preview.2) (2020-11-15)


### Features

* :sparkles: Add allocator API ([48abe4b](https://github.com/CareBoo/Blinq/commit/48abe4bf14fe43219b0c16bbe1fdbccc558ece5f))


### BREAKING CHANGES

* `Execute` API is removed in ValueSequence

# [2.0.0-preview.1](https://github.com/CareBoo/Blinq/compare/v1.0.0...v2.0.0-preview.1) (2020-10-26)


### Features

* :sparkles: Use `Burst.Delegates` ValueFuncs ([3207cdb](https://github.com/CareBoo/Blinq/commit/3207cdbaa9261a4081d72b57e3ba7a8881c4ed58)), closes [#55](https://github.com/CareBoo/Blinq/issues/55)


### BREAKING CHANGES

* ValueFunc API is now implemented by a package

# 1.0.0 (2020-10-13)


### Bug Fixes

* :bug: enforce IFunc struct params ([ca074fe](https://github.com/CareBoo/Blinq/commit/ca074fe5cb82d619e1c780557eac1b6a0298e38c))
* :bug: Fix concat memory leak ([73eaaae](https://github.com/CareBoo/Blinq/commit/73eaaaeb020dcd3d0902a8571aae502823dd9930))
* :bug: Fix Count using incorrect ValueFunc API ([a3849f8](https://github.com/CareBoo/Blinq/commit/a3849f86515ee9156794e810d94c2bf56a84abdd))
* :bug: Fix dispose in Count ([4d4e36e](https://github.com/CareBoo/Blinq/commit/4d4e36e16ae2d723aab621eae940b9534ffd5a49))
* :bug: Fix issue when single predicate matches ([83883c0](https://github.com/CareBoo/Blinq/commit/83883c02742dc747965ef667f7c57735471e8c54))
* :bug: Fix NativeArray.Join API ([1d49715](https://github.com/CareBoo/Blinq/commit/1d497154018ffc3bf88fbc241d51b2e468500545))
* :bug: fixing Average race condition ([366ecaa](https://github.com/CareBoo/Blinq/commit/366ecaabd868ce1816a7f8cd6489cdaec99c0435))
* :bug: remove deallocate ([9eefb9d](https://github.com/CareBoo/Blinq/commit/9eefb9d792536495c860c196fec9d96aa38a0c15))
* :bug: Remove deallocate for aggregate and count ([5b415fd](https://github.com/CareBoo/Blinq/commit/5b415fd826fbd52ca6a7700fd40ef80c602bcf93))


### Code Refactoring

* :art: replacing IMap with IFunc ([f9b6a18](https://github.com/CareBoo/Blinq/commit/f9b6a181f9eb2a7370066fba8c0a670f1a14a8d9))


### Features

* :sparkles: Add Aggregate IFunc API ([bc5ab35](https://github.com/CareBoo/Blinq/commit/bc5ab3569df4b9cd042ef499bf12836265dbd87a))
* :sparkles: Add CodeGenSourceApiAttribute ([f176c70](https://github.com/CareBoo/Blinq/commit/f176c7040c4b7584901c18cbc2cb3ab1cd4b39c4))
* :sparkles: Add CodeGenTargetApiAttribute ([44d3037](https://github.com/CareBoo/Blinq/commit/44d3037e0faf4941cf62ad1e3fa64da2119cac58))
* :sparkles: Add INativeDisposable interface ([d294b42](https://github.com/CareBoo/Blinq/commit/d294b42a65b89b2080c9145426f35aec068de527))
* :sparkles: Add IOrderedSequence interface ([d118a54](https://github.com/CareBoo/Blinq/commit/d118a54b07a8e1d3f21f18a96a99c0e6a11df2b5))
* :sparkles: Add Job-API methods to ValueSequence ([3d1f775](https://github.com/CareBoo/Blinq/commit/3d1f77565bd4ef5c536e04743bca899c88d4b4b9))
* :sparkles: Add Lots of Features ([ccdf51a](https://github.com/CareBoo/Blinq/commit/ccdf51a9ba2a1307414ec83edb02b8fc34d1aa9d))
* :sparkles: Add NativeArray impl of Union ([a41481d](https://github.com/CareBoo/Blinq/commit/a41481d6cc9bbebe5e5233312d15fbf58a7a556e))
* :sparkles: Add NotCodeGenerated error ([968e9bf](https://github.com/CareBoo/Blinq/commit/968e9bf5307bfa1dd7f220593315bc1bbb62b64b))
* :sparkles: Add ToNativeArray and ToNativeArrayJob ([22f7f15](https://github.com/CareBoo/Blinq/commit/22f7f15419accf7a65d3c56069dd760445fc2732)), closes [#31](https://github.com/CareBoo/Blinq/issues/31)
* :sparkles: Add ToNativeHashMap and ToNativeHashMapJob ([b176257](https://github.com/CareBoo/Blinq/commit/b17625750d231471933ef9d24d7a6728fd2b4eab))
* :sparkles: Add ToNativeList and ToNativeListJob ([64e7b32](https://github.com/CareBoo/Blinq/commit/64e7b3250104c606fc9120828b3bb3f9d405d89b))
* :sparkles: Adding Aggregate Method ([bcc8ee1](https://github.com/CareBoo/Blinq/commit/bcc8ee10b747bf67eb249030e07f9203cef8dcb0))
* :sparkles: Adding All Method ([b7c8858](https://github.com/CareBoo/Blinq/commit/b7c8858b66649e8f9df8afecba43e5abb17dad86))
* :sparkles: adding Any method ([8f5df36](https://github.com/CareBoo/Blinq/commit/8f5df364663b37614320be39d8c2617cce65f679))
* :sparkles: adding BFunc ([14c0d1b](https://github.com/CareBoo/Blinq/commit/14c0d1b325959760ad11d3be0beba928bf1d0621))
* :sparkles: adding Concat ([591a3bc](https://github.com/CareBoo/Blinq/commit/591a3bc0926327193428c745dacc55ff40bd4001))
* :sparkles: adding count ([982e113](https://github.com/CareBoo/Blinq/commit/982e113e604e3eb33e27147a739011dc7017d82e))
* :sparkles: adding in average ([1336d48](https://github.com/CareBoo/Blinq/commit/1336d481a33e6890a6cdf7e757e8d2aa2305ad14))
* :sparkles: adding in NativeSequence ([ef5f7d1](https://github.com/CareBoo/Blinq/commit/ef5f7d17af295c2ecab370f6ba0b8460d40046e0))
* :sparkles: adding predicate with 2 args ([0be9da7](https://github.com/CareBoo/Blinq/commit/0be9da7c284e52196e06d8f988857da12d988a62))
* :sparkles: Adding Select ([fcf1009](https://github.com/CareBoo/Blinq/commit/fcf10094a0d325a01a0d241bbc74d2d3c576edc1))
* :sparkles: Implement Average ([34e4b73](https://github.com/CareBoo/Blinq/commit/34e4b73ad94d5b719912fcd63031f445aae81307)), closes [#3](https://github.com/CareBoo/Blinq/issues/3)
* :sparkles: Implement CodeGenApi attributes ([3325f2d](https://github.com/CareBoo/Blinq/commit/3325f2de12e4d60000cf2b1f3c7f3d7d6d1d8fd4))
* :sparkles: Implement Contains ([42f6b6b](https://github.com/CareBoo/Blinq/commit/42f6b6b9804dbe362215053f4f4a29779140311b)), closes [#4](https://github.com/CareBoo/Blinq/issues/4)
* :sparkles: Implement Conversion for NativeArray ([654e5cd](https://github.com/CareBoo/Blinq/commit/654e5cd3489d5e39ab72d57d18869c052c11f2bb))
* :sparkles: Implement DefaultIfEmpty ([1b9db20](https://github.com/CareBoo/Blinq/commit/1b9db2037ef406740463f4296a9105bd4c3f3885)), closes [#5](https://github.com/CareBoo/Blinq/issues/5)
* :sparkles: Implement Distinct ([f44d7a8](https://github.com/CareBoo/Blinq/commit/f44d7a893493dca3b7477eae77a200a0fec55656)), closes [#6](https://github.com/CareBoo/Blinq/issues/6)
* :sparkles: Implement ElementAt and ElementAtOrDefault ([f63a2df](https://github.com/CareBoo/Blinq/commit/f63a2dfe7f32c790a54d1e3138559f0aaef2a4a3)), closes [#7](https://github.com/CareBoo/Blinq/issues/7)
* :sparkles: Implement Except ([2ed72b6](https://github.com/CareBoo/Blinq/commit/2ed72b6055d4994769da09e1a14adc7b601fa789)), closes [#9](https://github.com/CareBoo/Blinq/issues/9)
* :sparkles: Implement First and FirstOrDefault ([da6bc53](https://github.com/CareBoo/Blinq/commit/da6bc535d42b37f3f6cb1754bb1093786d5c3474)), closes [#10](https://github.com/CareBoo/Blinq/issues/10)
* :sparkles: Implement GroupJoin ([3a20703](https://github.com/CareBoo/Blinq/commit/3a207038a4ad4008e28671d8c347676616abc345)), closes [#12](https://github.com/CareBoo/Blinq/issues/12)
* :sparkles: Implement Intersect ([4d092c3](https://github.com/CareBoo/Blinq/commit/4d092c314b972ffe131d3c3803c9708e6f9986fa)), closes [#13](https://github.com/CareBoo/Blinq/issues/13)
* :sparkles: Implement Join ([81451a5](https://github.com/CareBoo/Blinq/commit/81451a52d8051837f97e523cd1dabbff8b3f27d4)), closes [#14](https://github.com/CareBoo/Blinq/issues/14)
* :sparkles: Implement Last and LastOrDefault ([31db989](https://github.com/CareBoo/Blinq/commit/31db9893a26e3f8eafd4096d9acf97f11feefd06)), closes [#15](https://github.com/CareBoo/Blinq/issues/15)
* :sparkles: Implement LongCount ([b4eff80](https://github.com/CareBoo/Blinq/commit/b4eff80359175266ddf4c01ffe78227b5e1bafc8)), closes [#16](https://github.com/CareBoo/Blinq/issues/16)
* :sparkles: Implement MaxMin ([8e803fe](https://github.com/CareBoo/Blinq/commit/8e803fe4a09b3a63d78501eb3809501a774804d8)), closes [#17](https://github.com/CareBoo/Blinq/issues/17)
* :sparkles: Implement OrderBy and OrderByDescending ([e15f42e](https://github.com/CareBoo/Blinq/commit/e15f42ebf82fba6cd3c9bf0e1f45fc5e3c7607a6)), closes [#18](https://github.com/CareBoo/Blinq/issues/18)
* :sparkles: Implement Prepend ([35ec722](https://github.com/CareBoo/Blinq/commit/35ec7226839b8a7f3ac41464b8bc0d1d88578a80)), closes [#19](https://github.com/CareBoo/Blinq/issues/19)
* :sparkles: Implement Reverse ([204fab5](https://github.com/CareBoo/Blinq/commit/204fab58d9b4483a72a00b6e760d16f53b1fd414)), closes [#22](https://github.com/CareBoo/Blinq/issues/22)
* :sparkles: Implement SelectMany ([43aaa5b](https://github.com/CareBoo/Blinq/commit/43aaa5b1858a17336728484bd47d342922f3ef23)), closes [#23](https://github.com/CareBoo/Blinq/issues/23)
* :sparkles: Implement Sequence.Empty ([ed2f9e1](https://github.com/CareBoo/Blinq/commit/ed2f9e15f5889ad3bde20aa211f2c03e308a9810)), closes [#8](https://github.com/CareBoo/Blinq/issues/8)
* :sparkles: Implement Sequence.Range ([b43345d](https://github.com/CareBoo/Blinq/commit/b43345d72644d5eff7926cd62b66c9032c61e750)), closes [#20](https://github.com/CareBoo/Blinq/issues/20)
* :sparkles: Implement Sequence.Repeat ([3d7ce42](https://github.com/CareBoo/Blinq/commit/3d7ce42cd32163f3d3a691fb19dca5ef0a38dfca)), closes [#21](https://github.com/CareBoo/Blinq/issues/21)
* :sparkles: Implement SequenceEqual ([706d21a](https://github.com/CareBoo/Blinq/commit/706d21af4bba1d78b02c57637b2e70f74e600d8e)), closes [#24](https://github.com/CareBoo/Blinq/issues/24)
* :sparkles: Implement Single and SingleOrDefault ([cc397a5](https://github.com/CareBoo/Blinq/commit/cc397a50610d763db88512d6c020f1e6c787fd7e)), closes [#25](https://github.com/CareBoo/Blinq/issues/25)
* :sparkles: Implement Skip and SkipWhile ([e3b459a](https://github.com/CareBoo/Blinq/commit/e3b459a3c4a4b8bfda55bbdad37f2398abba754a)), closes [#26](https://github.com/CareBoo/Blinq/issues/26)
* :sparkles: Implement subset of GroupBy ([220b62c](https://github.com/CareBoo/Blinq/commit/220b62c284f2a443d0a11612d291c5c9abdab342)), closes [#11](https://github.com/CareBoo/Blinq/issues/11)
* :sparkles: Implement Sum ([872e64e](https://github.com/CareBoo/Blinq/commit/872e64ed60112a74b078c7d30337ce869bbf5fad)), closes [#27](https://github.com/CareBoo/Blinq/issues/27)
* :sparkles: Implement Take and TakeWhile ([f2b9a9f](https://github.com/CareBoo/Blinq/commit/f2b9a9fa3564057fc0e41a5b7a5d3bc122cc9997)), closes [#28](https://github.com/CareBoo/Blinq/issues/28)
* :sparkles: Implement ThenBy ([9f9d50a](https://github.com/CareBoo/Blinq/commit/9f9d50a6fff4bbc7e97551117a5c42cf7d2b831c)), closes [#29](https://github.com/CareBoo/Blinq/issues/29)
* :sparkles: Implement Union ([205e4ca](https://github.com/CareBoo/Blinq/commit/205e4ca2d97aca7b7d91448bc5e3d261cc9be580)), closes [#32](https://github.com/CareBoo/Blinq/issues/32)
* :sparkles: Implement Zip ([36d847a](https://github.com/CareBoo/Blinq/commit/36d847afd6bb8613429be903672156a5283a4d42)), closes [#33](https://github.com/CareBoo/Blinq/issues/33)
* :sparkles: Reimpl ToNative* extension methods ([fab1b10](https://github.com/CareBoo/Blinq/commit/fab1b1007df5f4eb0a50924e102fc48fcd9b7ad9))
* :sparkles: Use NativeList as backed field ([5044aab](https://github.com/CareBoo/Blinq/commit/5044aab83a6be2e479d48a5177b6471f622da100))
* :tada: Implement ValueSequence API ([5d27a08](https://github.com/CareBoo/Blinq/commit/5d27a08987688759353ea6cb62ec0a66bde6b68e))
* :tada: Now managing memory conventionally ([0ffc964](https://github.com/CareBoo/Blinq/commit/0ffc964637b52fd2e519bbe8ee407b24a71a3267))


### Reverts

* :fire: Delete average ([c63fc35](https://github.com/CareBoo/Blinq/commit/c63fc35454ebbe8ae5f075b237ecbc10209d0b4d))


### BREAKING CHANGES

* Delete Native Sequence
* literally everything. About to remove NativeSequence
* Average is no longer supported.
* this whole repo got foobar'd. All the tests have been rewritten, and the extension methods have been removed to focus on getting Blinq working with a single struct for now.
* IMap removed, use IFunc instead

# [1.0.0-preview.24](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.23...v1.0.0-preview.24) (2020-09-18)


### Features

* :sparkles: Implement subset of GroupBy ([220b62c](https://github.com/CareBoo/Blinq/commit/220b62c284f2a443d0a11612d291c5c9abdab342)), closes [#11](https://github.com/CareBoo/Blinq/issues/11)

# [1.0.0-preview.23](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.22...v1.0.0-preview.23) (2020-09-18)


### Features

* :sparkles: Implement Average ([34e4b73](https://github.com/CareBoo/Blinq/commit/34e4b73ad94d5b719912fcd63031f445aae81307)), closes [#3](https://github.com/CareBoo/Blinq/issues/3)
* :sparkles: Implement MaxMin ([8e803fe](https://github.com/CareBoo/Blinq/commit/8e803fe4a09b3a63d78501eb3809501a774804d8)), closes [#17](https://github.com/CareBoo/Blinq/issues/17)
* :sparkles: Implement Sum ([872e64e](https://github.com/CareBoo/Blinq/commit/872e64ed60112a74b078c7d30337ce869bbf5fad)), closes [#27](https://github.com/CareBoo/Blinq/issues/27)

# [1.0.0-preview.22](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.21...v1.0.0-preview.22) (2020-09-17)


### Bug Fixes

* :bug: Fix NativeArray.Join API ([1d49715](https://github.com/CareBoo/Blinq/commit/1d497154018ffc3bf88fbc241d51b2e468500545))


### Features

* :sparkles: Implement GroupJoin ([3a20703](https://github.com/CareBoo/Blinq/commit/3a207038a4ad4008e28671d8c347676616abc345)), closes [#12](https://github.com/CareBoo/Blinq/issues/12)

# [1.0.0-preview.21](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.20...v1.0.0-preview.21) (2020-09-17)


### Features

* :sparkles: Implement SelectMany ([43aaa5b](https://github.com/CareBoo/Blinq/commit/43aaa5b1858a17336728484bd47d342922f3ef23)), closes [#23](https://github.com/CareBoo/Blinq/issues/23)

# [1.0.0-preview.20](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.19...v1.0.0-preview.20) (2020-09-16)


### Features

* :sparkles: Add IOrderedSequence interface ([d118a54](https://github.com/CareBoo/Blinq/commit/d118a54b07a8e1d3f21f18a96a99c0e6a11df2b5))
* :sparkles: Implement ThenBy ([9f9d50a](https://github.com/CareBoo/Blinq/commit/9f9d50a6fff4bbc7e97551117a5c42cf7d2b831c)), closes [#29](https://github.com/CareBoo/Blinq/issues/29)

# [1.0.0-preview.19](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.18...v1.0.0-preview.19) (2020-09-15)


### Features

* :sparkles: Implement Except ([2ed72b6](https://github.com/CareBoo/Blinq/commit/2ed72b6055d4994769da09e1a14adc7b601fa789)), closes [#9](https://github.com/CareBoo/Blinq/issues/9)
* :sparkles: Implement Intersect ([4d092c3](https://github.com/CareBoo/Blinq/commit/4d092c314b972ffe131d3c3803c9708e6f9986fa)), closes [#13](https://github.com/CareBoo/Blinq/issues/13)
* :sparkles: Implement Join ([81451a5](https://github.com/CareBoo/Blinq/commit/81451a52d8051837f97e523cd1dabbff8b3f27d4)), closes [#14](https://github.com/CareBoo/Blinq/issues/14)

# [1.0.0-preview.18](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.17...v1.0.0-preview.18) (2020-09-15)


### Bug Fixes

* :bug: Fix Count using incorrect ValueFunc API ([a3849f8](https://github.com/CareBoo/Blinq/commit/a3849f86515ee9156794e810d94c2bf56a84abdd))
* :bug: Fix dispose in Count ([4d4e36e](https://github.com/CareBoo/Blinq/commit/4d4e36e16ae2d723aab621eae940b9534ffd5a49))


### Features

* :sparkles: Implement LongCount ([b4eff80](https://github.com/CareBoo/Blinq/commit/b4eff80359175266ddf4c01ffe78227b5e1bafc8)), closes [#16](https://github.com/CareBoo/Blinq/issues/16)
* :sparkles: Implement OrderBy and OrderByDescending ([e15f42e](https://github.com/CareBoo/Blinq/commit/e15f42ebf82fba6cd3c9bf0e1f45fc5e3c7607a6)), closes [#18](https://github.com/CareBoo/Blinq/issues/18)
* :sparkles: Implement Prepend ([35ec722](https://github.com/CareBoo/Blinq/commit/35ec7226839b8a7f3ac41464b8bc0d1d88578a80)), closes [#19](https://github.com/CareBoo/Blinq/issues/19)
* :sparkles: Implement Reverse ([204fab5](https://github.com/CareBoo/Blinq/commit/204fab58d9b4483a72a00b6e760d16f53b1fd414)), closes [#22](https://github.com/CareBoo/Blinq/issues/22)
* :sparkles: Implement SequenceEqual ([706d21a](https://github.com/CareBoo/Blinq/commit/706d21af4bba1d78b02c57637b2e70f74e600d8e)), closes [#24](https://github.com/CareBoo/Blinq/issues/24)
* :sparkles: Implement Skip and SkipWhile ([e3b459a](https://github.com/CareBoo/Blinq/commit/e3b459a3c4a4b8bfda55bbdad37f2398abba754a)), closes [#26](https://github.com/CareBoo/Blinq/issues/26)
* :sparkles: Implement Take and TakeWhile ([f2b9a9f](https://github.com/CareBoo/Blinq/commit/f2b9a9fa3564057fc0e41a5b7a5d3bc122cc9997)), closes [#28](https://github.com/CareBoo/Blinq/issues/28)

# [1.0.0-preview.17](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.16...v1.0.0-preview.17) (2020-09-14)


### Bug Fixes

* :bug: Fix issue when single predicate matches ([83883c0](https://github.com/CareBoo/Blinq/commit/83883c02742dc747965ef667f7c57735471e8c54))


### Features

* :sparkles: Implement First and FirstOrDefault ([da6bc53](https://github.com/CareBoo/Blinq/commit/da6bc535d42b37f3f6cb1754bb1093786d5c3474)), closes [#10](https://github.com/CareBoo/Blinq/issues/10)
* :sparkles: Implement Last and LastOrDefault ([31db989](https://github.com/CareBoo/Blinq/commit/31db9893a26e3f8eafd4096d9acf97f11feefd06)), closes [#15](https://github.com/CareBoo/Blinq/issues/15)
* :sparkles: Implement Single and SingleOrDefault ([cc397a5](https://github.com/CareBoo/Blinq/commit/cc397a50610d763db88512d6c020f1e6c787fd7e)), closes [#25](https://github.com/CareBoo/Blinq/issues/25)

# [1.0.0-preview.16](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.15...v1.0.0-preview.16) (2020-09-14)


### Features

* :sparkles: Implement Conversion for NativeArray ([654e5cd](https://github.com/CareBoo/Blinq/commit/654e5cd3489d5e39ab72d57d18869c052c11f2bb))
* :sparkles: Implement Sequence.Empty ([ed2f9e1](https://github.com/CareBoo/Blinq/commit/ed2f9e15f5889ad3bde20aa211f2c03e308a9810)), closes [#8](https://github.com/CareBoo/Blinq/issues/8)
* :sparkles: Implement Sequence.Range ([b43345d](https://github.com/CareBoo/Blinq/commit/b43345d72644d5eff7926cd62b66c9032c61e750)), closes [#20](https://github.com/CareBoo/Blinq/issues/20)
* :sparkles: Implement Sequence.Repeat ([3d7ce42](https://github.com/CareBoo/Blinq/commit/3d7ce42cd32163f3d3a691fb19dca5ef0a38dfca)), closes [#21](https://github.com/CareBoo/Blinq/issues/21)

# [1.0.0-preview.15](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.14...v1.0.0-preview.15) (2020-09-14)


### Features

* :sparkles: Implement ElementAt and ElementAtOrDefault ([f63a2df](https://github.com/CareBoo/Blinq/commit/f63a2dfe7f32c790a54d1e3138559f0aaef2a4a3)), closes [#7](https://github.com/CareBoo/Blinq/issues/7)

# [1.0.0-preview.14](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.13...v1.0.0-preview.14) (2020-09-14)


### Features

* :sparkles: Implement Distinct ([f44d7a8](https://github.com/CareBoo/Blinq/commit/f44d7a893493dca3b7477eae77a200a0fec55656)), closes [#6](https://github.com/CareBoo/Blinq/issues/6)

# [1.0.0-preview.13](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.12...v1.0.0-preview.13) (2020-09-14)


### Features

* :sparkles: Add Job-API methods to ValueSequence ([3d1f775](https://github.com/CareBoo/Blinq/commit/3d1f77565bd4ef5c536e04743bca899c88d4b4b9))
* :sparkles: Reimpl ToNative* extension methods ([fab1b10](https://github.com/CareBoo/Blinq/commit/fab1b1007df5f4eb0a50924e102fc48fcd9b7ad9))

# [1.0.0-preview.12](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.11...v1.0.0-preview.12) (2020-09-10)


### Features

* :sparkles: Implement DefaultIfEmpty ([1b9db20](https://github.com/CareBoo/Blinq/commit/1b9db2037ef406740463f4296a9105bd4c3f3885)), closes [#5](https://github.com/CareBoo/Blinq/issues/5)

# [1.0.0-preview.11](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.10...v1.0.0-preview.11) (2020-09-10)


### Features

* :sparkles: Implement Contains ([42f6b6b](https://github.com/CareBoo/Blinq/commit/42f6b6b9804dbe362215053f4f4a29779140311b)), closes [#4](https://github.com/CareBoo/Blinq/issues/4)

# [1.0.0-preview.10](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.9...v1.0.0-preview.10) (2020-09-10)


### Features

* :sparkles: Add NativeArray impl of Union ([a41481d](https://github.com/CareBoo/Blinq/commit/a41481d6cc9bbebe5e5233312d15fbf58a7a556e))
* :sparkles: Implement Union ([205e4ca](https://github.com/CareBoo/Blinq/commit/205e4ca2d97aca7b7d91448bc5e3d261cc9be580)), closes [#32](https://github.com/CareBoo/Blinq/issues/32)

# [1.0.0-preview.9](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.8...v1.0.0-preview.9) (2020-09-09)


### Features

* :sparkles: Add ToNativeArray and ToNativeArrayJob ([22f7f15](https://github.com/CareBoo/Blinq/commit/22f7f15419accf7a65d3c56069dd760445fc2732)), closes [#31](https://github.com/CareBoo/Blinq/issues/31)
* :sparkles: Add ToNativeHashMap and ToNativeHashMapJob ([b176257](https://github.com/CareBoo/Blinq/commit/b17625750d231471933ef9d24d7a6728fd2b4eab))
* :sparkles: Add ToNativeList and ToNativeListJob ([64e7b32](https://github.com/CareBoo/Blinq/commit/64e7b3250104c606fc9120828b3bb3f9d405d89b))

# [1.0.0-preview.8](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.7...v1.0.0-preview.8) (2020-09-04)


### Bug Fixes

* :bug: Fix concat memory leak ([73eaaae](https://github.com/CareBoo/Blinq/commit/73eaaaeb020dcd3d0902a8571aae502823dd9930))


### Features

* :sparkles: Implement Zip ([36d847a](https://github.com/CareBoo/Blinq/commit/36d847afd6bb8613429be903672156a5283a4d42)), closes [#33](https://github.com/CareBoo/Blinq/issues/33)

# [1.0.0-preview.7](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.6...v1.0.0-preview.7) (2020-09-04)


### Bug Fixes

* :bug: enforce IFunc struct params ([ca074fe](https://github.com/CareBoo/Blinq/commit/ca074fe5cb82d619e1c780557eac1b6a0298e38c))
* :bug: remove deallocate ([9eefb9d](https://github.com/CareBoo/Blinq/commit/9eefb9d792536495c860c196fec9d96aa38a0c15))
* :bug: Remove deallocate for aggregate and count ([5b415fd](https://github.com/CareBoo/Blinq/commit/5b415fd826fbd52ca6a7700fd40ef80c602bcf93))


### Features

* :sparkles: Add CodeGenSourceApiAttribute ([f176c70](https://github.com/CareBoo/Blinq/commit/f176c7040c4b7584901c18cbc2cb3ab1cd4b39c4))
* :sparkles: Add CodeGenTargetApiAttribute ([44d3037](https://github.com/CareBoo/Blinq/commit/44d3037e0faf4941cf62ad1e3fa64da2119cac58))
* :sparkles: Add INativeDisposable interface ([d294b42](https://github.com/CareBoo/Blinq/commit/d294b42a65b89b2080c9145426f35aec068de527))
* :sparkles: Add Lots of Features ([ccdf51a](https://github.com/CareBoo/Blinq/commit/ccdf51a9ba2a1307414ec83edb02b8fc34d1aa9d))
* :sparkles: Implement CodeGenApi attributes ([3325f2d](https://github.com/CareBoo/Blinq/commit/3325f2de12e4d60000cf2b1f3c7f3d7d6d1d8fd4))
* :sparkles: Use NativeList as backed field ([5044aab](https://github.com/CareBoo/Blinq/commit/5044aab83a6be2e479d48a5177b6471f622da100))
* :tada: Implement ValueSequence API ([5d27a08](https://github.com/CareBoo/Blinq/commit/5d27a08987688759353ea6cb62ec0a66bde6b68e))


### BREAKING CHANGES

* Delete Native Sequence
* literally everything. About to remove NativeSequence

# [1.0.0-preview.6](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.5...v1.0.0-preview.6) (2020-08-29)


### Features

* :sparkles: Add Aggregate IFunc API ([bc5ab35](https://github.com/CareBoo/Blinq/commit/bc5ab3569df4b9cd042ef499bf12836265dbd87a))
* :sparkles: Add NotCodeGenerated error ([968e9bf](https://github.com/CareBoo/Blinq/commit/968e9bf5307bfa1dd7f220593315bc1bbb62b64b))


### Reverts

* :fire: Delete average ([c63fc35](https://github.com/CareBoo/Blinq/commit/c63fc35454ebbe8ae5f075b237ecbc10209d0b4d))


### BREAKING CHANGES

* Average is no longer supported.

# [1.0.0-preview.5](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.4...v1.0.0-preview.5) (2020-08-27)


### Bug Fixes

* :bug: fixing Average race condition ([366ecaa](https://github.com/CareBoo/Blinq/commit/366ecaabd868ce1816a7f8cd6489cdaec99c0435))


### Features

* :sparkles: adding Concat ([591a3bc](https://github.com/CareBoo/Blinq/commit/591a3bc0926327193428c745dacc55ff40bd4001))
* :sparkles: adding count ([982e113](https://github.com/CareBoo/Blinq/commit/982e113e604e3eb33e27147a739011dc7017d82e))

# [1.0.0-preview.4](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.3...v1.0.0-preview.4) (2020-08-27)


### Features

* :sparkles: adding in average ([1336d48](https://github.com/CareBoo/Blinq/commit/1336d481a33e6890a6cdf7e757e8d2aa2305ad14))

# [1.0.0-preview.3](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.2...v1.0.0-preview.3) (2020-08-26)


### Features

* :sparkles: adding Any method ([8f5df36](https://github.com/CareBoo/Blinq/commit/8f5df364663b37614320be39d8c2617cce65f679))
* :sparkles: adding BFunc ([14c0d1b](https://github.com/CareBoo/Blinq/commit/14c0d1b325959760ad11d3be0beba928bf1d0621))
* :sparkles: adding in NativeSequence ([ef5f7d1](https://github.com/CareBoo/Blinq/commit/ef5f7d17af295c2ecab370f6ba0b8460d40046e0))
* :sparkles: adding predicate with 2 args ([0be9da7](https://github.com/CareBoo/Blinq/commit/0be9da7c284e52196e06d8f988857da12d988a62))
* :sparkles: Adding Select ([fcf1009](https://github.com/CareBoo/Blinq/commit/fcf10094a0d325a01a0d241bbc74d2d3c576edc1))
* :tada: Now managing memory conventionally ([0ffc964](https://github.com/CareBoo/Blinq/commit/0ffc964637b52fd2e519bbe8ee407b24a71a3267))


### BREAKING CHANGES

* this whole repo got foobar'd. All the tests have been rewritten, and the extension methods have been removed to focus on getting Blinq working with a single struct for now.

# [1.0.0-preview.2](https://github.com/CareBoo/Blinq/compare/v1.0.0-preview.1...v1.0.0-preview.2) (2020-08-23)


### Code Refactoring

* :art: replacing IMap with IFunc ([f9b6a18](https://github.com/CareBoo/Blinq/commit/f9b6a181f9eb2a7370066fba8c0a670f1a14a8d9))


### BREAKING CHANGES

* IMap removed, use IFunc instead

# 1.0.0-preview.1 (2020-08-22)


### Features

* :sparkles: Adding Aggregate Method ([bcc8ee1](https://github.com/CareBoo/BLinq/commit/bcc8ee10b747bf67eb249030e07f9203cef8dcb0))
* :sparkles: Adding All Method ([b7c8858](https://github.com/CareBoo/BLinq/commit/b7c8858b66649e8f9df8afecba43e5abb17dad86))

# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2020-08-18

### This is the first release of *\<My Package\>*.

*Short description of this release*
