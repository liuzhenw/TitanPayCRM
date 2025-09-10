<template>
  <FullScreen>
    <div id="container">
      <el-page-header @back="goBack">
        <template #content>
          <span class="text-large font-600 mr-3"> 销售记录详情 </span>
        </template>
      </el-page-header>

      <div v-loading="!saleLog" class="content">
        <el-row v-if="saleLog" :gutter="16">
          <el-col :md="6" :sm="24">
            <el-card shadow="never" style="height: 100%">
              <template #title>
                <span class="text-large font-600"> 商品信息 </span>
              </template>
              <Basic :saleLog="saleLog" />
            </el-card>
          </el-col>
          <el-col :md="18" :sm="24">
            <el-card shadow="never">
              <template #title>
                <span class="text-large font-600"> 佣金记录 </span>
              </template>
              <CommissionLogs :saleLog="saleLog" />
            </el-card>
          </el-col>
        </el-row>
      </div>
    </div>
  </FullScreen>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { ElMessage } from 'element-plus'
  import { ProductSaleLogService, ProductSaleLogDto } from '@/api/services'
  import Basic from './basic.vue'
  import CommissionLogs from './commissions.vue'

  const router = useRouter()
  const route = useRoute()
  const saleLogId = route.params.id as string
  const saleLog = ref<ProductSaleLogDto>()

  const goBack = () => {
    router.back()
  }

  onMounted(async () => {
    try {
      saleLog.value = await ProductSaleLogService.get(saleLogId)
    } catch (error) {
      ElMessage.error('获取销售记录详情失败')
      router.back()
    }
  })
</script>

<style scoped lang="scss">
  .content {
    margin-top: 20px;
  }
</style>
