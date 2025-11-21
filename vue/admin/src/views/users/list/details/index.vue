<template>
  <div>
    <el-page-header @back="() => $router.back()">
      <template #content> 用户详情 </template>
    </el-page-header>

    <div class="user-details">
      <el-skeleton v-if="!user" :rows="4" />
      <div v-else>
        <el-row :gutter="16" class="equal-height-row">
          <el-col :md="12" :sm="24" class="equal-height-col">
            <Basic v-if="user" :value="user" class="full-height-card" />
          </el-col>
          <el-col :md="12" :sm="24" class="equal-height-col">
            <Ancestors
              v-if="user"
              :user="user"
              class="full-height-card"
              @change="onAncestorsChange"
            />
          </el-col>
        </el-row>
        <el-row style="margin-top: 16px">
          <el-col :md="24">
            <ReferralRelations :user-id="user.id" />
          </el-col>
        </el-row>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import { UserService, UserWithDetailsDto } from '@/api/services'
  import Basic from './Basic.vue'
  import Ancestors from './Ancestors.vue'
  import ReferralRelations from './ReferralRelations.vue'

  const route = useRoute()
  const userId = route.params.id as string
  const user = ref<UserWithDetailsDto>()

  const onAncestorsChange = async () => {
    user.value = undefined
    user.value = await UserService.get(userId)
  }
  onMounted(async () => {
    try {
      user.value = await UserService.get(userId)
    } catch (error) {
      console.error('获取用户信息失败:', error)
    }
  })
</script>

<style scoped lang="scss">
  .user-details {
    margin-top: 16px;
    .equal-height-row {
      display: flex;
      flex-wrap: wrap;
    }

    .equal-height-col {
      display: flex;
      flex-direction: column;

      // 在中等屏幕及以上，确保两列等高
      @media (min-width: 768px) {
        flex: 1;
      }
    }

    .full-height-card {
      height: 100%;
      display: flex;
      flex-direction: column;

      // 确保 el-card 也占满高度
      :deep(.el-card) {
        height: 100%;
        display: flex;
        flex-direction: column;

        .el-card__body {
          flex: 1;
          display: flex;
          flex-direction: column;
        }
      }
    }

    .panel {
      margin-bottom: 16px;
      transition: all 0.3s ease;

      &:hover {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      }
    }
  }

  // 响应式设计
  @media (max-width: 768px) {
    .user-details {
      .equal-height-row {
        flex-direction: column;
      }

      .equal-height-col {
        flex: none;
      }

      .full-height-card {
        height: auto;

        :deep(.el-card) {
          height: auto;
        }
      }

      .panel {
        margin-bottom: 12px;
      }
    }
  }
</style>
