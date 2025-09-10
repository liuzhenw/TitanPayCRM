<template>
  <div id="container">
    <el-page-header @back="goBack">
      <template #content>
        <span class="text-large font-600 mr-3"> 推荐人详情 </span>
      </template>
    </el-page-header>

    <div v-loading="!referrer" class="content">
      <el-row v-if="referrer" :gutter="16">
        <el-col :md="8" :sm="24">
          <el-card shadow="never" class="panel">
            <template #header>
              <span class="text-large font-600"> 基本信息 </span>
            </template>
            <Basic :referrer="referrer" />
          </el-card>
        </el-col>
        <el-col :md="16" :sm="24">
          <el-row :gutter="16">
            <el-col :span="24">
              <el-card shadow="never" class="panel" >
                <template #header>
                  <span class="text-large font-600"> 推荐统计 </span>
                </template>
                <Statistics :referrer="referrer" />
              </el-card>
            </el-col>

            <el-col :span="24">
              <el-card shadow="never" class="panel" >
                <template #header>
                  <span class="text-large font-600"> 下级用户 </span>
                </template>
                <Recommendees :referrer="referrer" />
              </el-card>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { ElMessage } from 'element-plus'
  import { ReferrerService, ReferrerWithDetailsDto } from '@/api/services'
  import Basic from './basic.vue'
  import Statistics from './statistics.vue'
  import Recommendees from './recommendees.vue'

  const router = useRouter()
  const route = useRoute()
  const referrerId = route.params.id as string
  const referrer = ref<ReferrerWithDetailsDto>()

  const goBack = () => {
    router.back()
  }

  onMounted(async () => {
    try {
      referrer.value = await ReferrerService.get(referrerId)
    } catch (error) {
      ElMessage.error('获取推荐人详情失败')
      router.back()
    }
  })
</script>

<style scoped lang="scss">
  .content {
    margin-top: 20px;
    height: 100%;
  }
  .panel {
    margin-bottom: 16px;
  }
</style>
