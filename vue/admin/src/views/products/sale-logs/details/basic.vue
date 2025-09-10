<template>
  <el-form label-width="auto" label-suffix=":" style="min-width: 400px">
    <el-form-item label="产品名称">
      {{ saleLog.product.name }}
    </el-form-item>
    <el-form-item label="购买用户">
      {{ saleLog.customer.name }}
    </el-form-item>
    <el-form-item label="订单号">
      {{ saleLog.orderNo }}
    </el-form-item>
    <el-form-item label="销售数量">
      {{ saleLog.quantity }}
    </el-form-item>
    <el-form-item label="销售金额"> {{ saleLog.amount }} USD </el-form-item>
    <el-form-item label="总佣金"> {{ saleLog.totalCommission }} USD </el-form-item>
    <el-form-item label="创建时间">
      <Datetime :value="saleLog.createdAt" />
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
  import { ProductSaleLogDto } from '@/api/services'

  const { saleLog } = defineProps<{
    saleLog: ProductSaleLogDto
  }>()
</script>

<style scoped lang="scss">
  .warp {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
</style>
