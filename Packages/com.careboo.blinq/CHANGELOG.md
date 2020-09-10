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
