<template>
  <el-button
    v-if="!signer"
    v-bind="attrs"
    v-ripple
    :type="props.type"
    :icon="props.icon"
    :loading="connectLoading"
    :disabled="disabled"
    @click="connectWallet"
  >
    连接钱包
  </el-button>
  <el-space v-else>
    <EllipticalLabel :value="signer.address" :prefix-lenght="6" :suffix-lenght="6" :copy="false" />
    <el-button type="primary" link icon="Switch" @click="changeAccount"> 切换账号 </el-button>
  </el-space>
</template>

<script setup lang="ts">
  import { ButtonProps, ElButton, ElMessageBox } from 'element-plus'
  import { ethers, BrowserProvider, JsonRpcSigner } from 'ethers'
  type Props = {
    chain?: { networkId: string; name: string }
    address?: string
  }
  const attrs = useAttrs()
  const props = withDefaults(defineProps<Partial<ButtonProps> & Props>(), {
    type: 'success',
    icon: 'Wallet'
  })
  const emit = defineEmits(['connected', 'disconnected'])

  let provider: BrowserProvider | undefined
  const signer = ref<JsonRpcSigner>()
  const disabled = computed(() => {
    if (props.disabled === true) return true
    return window.ethereum == undefined
  })

  const chainId = computed(() => {
    if (!props.chain?.networkId) return 0n
    return BigInt(props.chain.networkId)
  })
  const networkId = computed(() => {
    return '0x' + chainId.value.toString(16)
  })

  const connectLoading = ref(false)
  const connectWallet = async () => {
    connectLoading.value = true
    try {
      provider = new ethers.BrowserProvider(window.ethereum)
      await ensureNetwork()
      signer.value = await provider.getSigner(props.address)
    } finally {
      connectLoading.value = false
    }
  }
  const ensureNetwork = async () => {
    if (chainId.value <= 0n) return

    const network = await provider!.getNetwork()
    if (network.chainId !== chainId.value) await switchNetwork()
  }
  const switchNetwork = async () => {
    try {
      await provider?.send('wallet_switchEthereumChain', [{ chainId: networkId.value }])
    } catch (e: any) {
      if (e.error?.code === 4902) {
        ElMessageBox.alert(`请在钱包中添加${props.chain!.name}网络!`),
          {
            type: 'error'
          }
        return
      }

      ElMessageBox.alert(`请将钱包网络切换到 ${props.chain!.name} !`, {
        type: 'error'
      })
      throw e
    }
  }
  const changeAccount = async () => {
    await provider!.send('wallet_requestPermissions', [{ eth_accounts: {} }])
  }

  const onAccountsChanged = async (accounts: string[]) => {
    if (props.address) {
      if (accounts.findIndex((x) => x.toLowerCase() == props.address!.toLowerCase()) < 0) {
        signer.value = undefined
        provider = undefined
      }
    } else {
      if (accounts.length <= 0) {
        signer.value = undefined
        provider = undefined
      } else {
        if (accounts.findIndex((x) => x == signer.value?.address) < 0) {
          signer.value = await provider?.getSigner()
        }
      }
    }

    if (signer.value) {
      await ensureNetwork()
    }
  }
  const onChainChanged = async (_: string) => {
    try {
      await ensureNetwork()
    } catch (e) {
      signer.value = undefined
    }
  }

  watch(
    () => signer.value,
    async () => {
      if (signer.value) emit('connected', toRaw(signer.value))
      else emit('disconnected')
    }
  )
  watch(
    () => props.chain,
    async () => {
      if (!signer.value) return
      try {
        await ensureNetwork()
      } catch (e) {
        signer.value = undefined
      }
    }
  )
  watch(
    () => props.address,
    async () => {
      if (!signer.value) return
      try {
        signer.value = await provider!.getSigner(props.address)
      } catch (e) {
        console.error(e)
        signer.value = undefined
      }
    }
  )

  onMounted(() => {
    if (window.ethereum) {
      window.ethereum.on('accountsChanged', onAccountsChanged)
      window.ethereum.on('chainChanged', onChainChanged)
    }
  })
  onUnmounted(() => {
    window.ethereum?.off('accountsChanged', onAccountsChanged)
    window.ethereum?.off('chainChanged', onChainChanged)
  })
</script>

<style scoped lang="scss"></style>
